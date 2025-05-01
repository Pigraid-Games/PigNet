
namespace PigNet.Net.EnumerationsTable;

public enum MolangVersion
{
	Invalid = -1,
	BeforeVersioning = 0,
	Initial = 1,
	FixedItemRemainingUseDurationQuery = 2,
	ExpressionErrorMessages = 3,
	UnexpectedOperatorErrors = 4,
	ConditionalOperatorAssociativity = 5,
	ComparisonAndLogicalOperatorPrecedence = 6,
	DivideByNegativeValue = 7,
	FixedCapeFlapAmountQuery = 8,
	QueryBlockPropertyRenamedToState = 9,
	DeprecateOldBlockQueryNames = 10,
	DeprecatedSnifferAndCamelQueries = 11,
	LeafSupportingInFirstSolidBlockBelow = 12,
	NumValidVersions = 13,
	Latest = NumValidVersions - 1,
	HardcodedMolang = Latest
}