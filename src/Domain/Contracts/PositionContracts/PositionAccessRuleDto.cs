using Domain.Enums;

namespace Domain.Contracts.PositionContracts;

public record PositionAccessRuleDto(
    int Id,
    int AttributeDefinitionId,
    string AttributeName,
    Operator Operator,
    string Value
);
