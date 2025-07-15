using QueryKit.Operators;

namespace QueryKit.Operators
{
    public static class OperatorTypeHelper
    {
        public static bool IsStringOperator(ComparisonOperator op)
        {
            var typeName = op.GetType().Name;
            return
                typeName == "ContainsType" ||
                typeName == "StartsWithType" ||
                typeName == "EndsWithType" ||
                typeName == "NotContainsType" ||
                typeName == "NotStartsWithType" ||
                typeName == "NotEndsWithType" ||
                typeName == "SoundsLikeType" ||
                typeName == "DoesNotSoundLikeType";
        }
    }
} 