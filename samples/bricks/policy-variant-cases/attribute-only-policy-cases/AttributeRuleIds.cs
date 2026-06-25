using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public static class AttributeRuleIds
{
    public const string OrdersPrefix = "ATTR-ORDERS-";
    public const string PaymentsPrefix = "ATTR-PAYMENTS-";
    public const string OrdersDomainMustNotUseInfrastructure = "ATTR-ORDERS-001";
    public const string OrdersHandlerRequiresRepositoryContract = "ATTR-ORDERS-002";
    public const string PaymentsNamespaceMustNotUseExternalGateway = "ATTR-PAYMENTS-001";
}
