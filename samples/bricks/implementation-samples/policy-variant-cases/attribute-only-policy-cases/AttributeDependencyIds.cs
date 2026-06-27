using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public static class AttributeDependencyIds
{
    public const string OrdersPrefix = "ATTR-ORDERS-DEP-";
    public const string PaymentsPrefix = "ATTR-PAYMENTS-DEP-";
    public const string OrderAggregateToSqlAdapter = "ATTR-ORDERS-DEP-001";
    public const string OrderHandlerToRepositoryContract = "ATTR-ORDERS-DEP-002";
    public const string PaymentNamespaceToGateway = "ATTR-PAYMENTS-DEP-001";
}
