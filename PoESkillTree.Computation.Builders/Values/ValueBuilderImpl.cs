﻿using System;
using System.Linq.Expressions;
using PoESkillTree.Common.Utils.Extensions;
using PoESkillTree.Computation.Builders.Conditions;
using PoESkillTree.Computation.Common;
using PoESkillTree.Computation.Common.Builders.Conditions;
using PoESkillTree.Computation.Common.Builders.Resolving;
using PoESkillTree.Computation.Common.Builders.Values;

namespace PoESkillTree.Computation.Builders.Values
{
    // ("Impl" suffix to avoid confusion with ValueBuilder in Common)
    public class ValueBuilderImpl : IValueBuilder
    {
        private readonly Func<Entity, IValue> _buildValue;
        private readonly Func<ResolveContext, IValueBuilder> _resolve;

        public ValueBuilderImpl(double? value) : this(new Constant(value))
        {
        }

        public ValueBuilderImpl(IValue value) : this(_ => value)
        {
        }

        private ValueBuilderImpl(Func<Entity, IValue> buildValue)
        {
            _buildValue = buildValue;
            _resolve = _ => this;
        }

        public ValueBuilderImpl(Func<Entity, IValue> buildValue, Func<ResolveContext, Func<Entity, IValue>> resolve)
            : this(buildValue, c => new ValueBuilderImpl(resolve(c)))
        {
        }

        public ValueBuilderImpl(Func<Entity, IValue> buildValue, Func<ResolveContext, IValueBuilder> resolve)
        {
            _buildValue = buildValue;
            _resolve = resolve;
        }

        public IValueBuilder Resolve(ResolveContext context) => _resolve(context);

        public IValueBuilder MaximumOnly =>
            Create(this, o => o.Select(v => new NodeValue(0, v.Maximum)), o => $"{o}.MaximumOnly");

        public IConditionBuilder Eq(IValueBuilder other) =>
            ValueConditionBuilder.Create(this, other, (left, right) => left == right);

        public IConditionBuilder GreaterThan(IValueBuilder other) =>
            ValueConditionBuilder.Create(this, other, (left, right) => left > right);

        public IValueBuilder Add(IValueBuilder other) =>
            Create(this, other, (left, right) => new[] { left, right }.Sum(), (l, r) => $"{l} + {r}");

        public IValueBuilder Multiply(IValueBuilder other) =>
            Create(this, other, (left, right) => new[] { left, right }.Product(), (l, r) => $"{l} * {r}");

        public IValueBuilder DivideBy(IValueBuilder divisor) =>
            Create(this, divisor, (left, right) => left / (right ?? new NodeValue(1)));

        public IValueBuilder Select(Expression<Func<double, double>> selector) =>
            Create(this, o => o.Select(selector.Compile()), o => selector.ToString(o));

        public IValueBuilder Create(double value) => new ValueBuilderImpl(value);

        public IValue Build(Entity modifierSourceEntity) => _buildValue(modifierSourceEntity);

        // TODO Only here for compatibility with stubs in Console. Remove once those are removed.
        public override string ToString() => Build(Entity.Character).ToString();


        public static IValueBuilder Create(
            IValueBuilder operand,
            Expression<Func<NodeValue?, NodeValue?>> calculate,
            Func<IValue, string> identityOverride = null) =>
            new ValueBuilderImpl(
                entity => Build(entity, operand, calculate, identityOverride),
                c => (entity => Build(entity, operand.Resolve(c), calculate, identityOverride)));

        public static IValueBuilder Create(
            IValueBuilder operand1, IValueBuilder operand2,
            Expression<Func<NodeValue?, NodeValue?, NodeValue?>> calculate,
            Func<IValue, IValue, string> identityOverride = null) =>
            new ValueBuilderImpl(
                entity => Build(entity, operand1, operand2, calculate, identityOverride),
                c => (entity => Build(entity, operand1.Resolve(c), operand2.Resolve(c), calculate, identityOverride)));

        private static IValue Build(
            Entity modifierSourceEntity,
            IValueBuilder operand,
            Expression<Func<NodeValue?, NodeValue?>> calculate,
            Func<IValue, string> identityOverride = null)
        {
            var builtOperand = operand.Build(modifierSourceEntity);
            var func = calculate.Compile();
            var identity = identityOverride is null ? calculate.ToString(builtOperand) : identityOverride(builtOperand);
            return new FunctionalValue(c => func(builtOperand.Calculate(c)), identity);
        }

        private static IValue Build(
            Entity modifierSourceEntity,
            IValueBuilder operand1, IValueBuilder operand2,
            Expression<Func<NodeValue?, NodeValue?, NodeValue?>> calculate,
            Func<IValue, IValue, string> identityOverride = null)
        {
            var o1 = operand1.Build(modifierSourceEntity);
            var o2 = operand2.Build(modifierSourceEntity);
            var func = calculate.Compile();
            var identity = identityOverride is null ? calculate.ToString(o1, o2) : identityOverride(o1, o2);
            return new FunctionalValue(c => func(o1.Calculate(c), o2.Calculate(c)), identity);
        }
    }
}