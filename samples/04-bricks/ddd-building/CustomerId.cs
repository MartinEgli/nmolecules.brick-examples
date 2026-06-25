using System;

namespace Samples.Block04.Bricks.DddBuilding;

[DddValueObject]
internal readonly record struct CustomerId(Guid Value);
