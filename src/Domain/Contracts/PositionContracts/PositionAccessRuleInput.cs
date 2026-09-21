using Domain.Enums;

namespace Domain.Contracts.PositionContracts;

public record PositionAccessRuleInput(
    int AttributeDefinitionId,
    Operator Operator,
    string Value
);
