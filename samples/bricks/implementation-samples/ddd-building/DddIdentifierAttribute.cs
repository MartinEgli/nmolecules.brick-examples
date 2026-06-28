using System;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class DddIdentifierAttribute : Attribute
{
}
