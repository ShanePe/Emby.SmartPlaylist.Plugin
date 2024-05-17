using System;
using System.Linq;
using SmartPlaylist.Contracts;
using SmartPlaylist.Domain.CriteriaDefinition;
using SmartPlaylist.Domain.Operator;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Adapters
{
    public static class RuleAdapter
    {
        public static RuleGroup Adapt(RuleTreeNodeDto[] allNodes)
        {
            return AdaptRule(allNodes.First(x => x.Data.Kind == RuleGroup.Kind), allNodes) as RuleGroup;
        }

        private static RuleBase AdaptRule(RuleTreeNodeDto node, RuleTreeNodeDto[] allNodes)
        {
            if (node.Data.Kind == RuleGroup.Kind)
                return new RuleGroup(node.Data.Id,
                    GetRules(node, allNodes), GetMatchMode(node.Data.MatchMode));

            return new Rule(node.Id, CreateCriteriaValue(node.Data.Criteria));
        }

        private static RuleBase[] GetRules(RuleTreeNodeDto node, RuleTreeNodeDto[] allNodes)
        {
            return node.Children.Select(x => GetNode(x, allNodes)).Select(x => AdaptRule(x, allNodes)).ToArray();
        }

        private static RuleGroupMatchMode GetMatchMode(string matchModeName)
        {
            if (Enum.TryParse(matchModeName, true, out RuleGroupMatchMode matchMode)) return matchMode;

            return RuleGroupMatchMode.All;
        }

        private static RuleCriteriaValue CreateCriteriaValue(RuleCriteriaValueDto dto)
        {
            var criteriaDefinition = DefinedCriteriaDefinitions.All.FirstOrDefault(x => x.Name == dto.Name);
            var operatorObj = GetOperator(dto.Operator);
            return new RuleCriteriaValue(dto.Value as Value, operatorObj, dto.UserId, criteriaDefinition);
        }

        private static Operator GetOperator(OperatorDto dto)
        {
            return DefinedOperators.All.FirstOrDefault(x => x.Name == dto.Name && x.Type == dto.Type);
        }

        private static RuleTreeNodeDto GetNode(string id, RuleTreeNodeDto[] allRulesTreeDtos)
        {
            return allRulesTreeDtos.First(x => x.Id == id);
        }
    }
}