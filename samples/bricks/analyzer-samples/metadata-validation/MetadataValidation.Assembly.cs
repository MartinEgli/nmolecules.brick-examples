using NMolecules.Bricks;

[assembly: Policy("orders-policy", "Orders architecture policy")]
[assembly: Policy("payments-policy", "Payments architecture policy")]
[assembly: Rule("ORDERS001", "Domain", "Infrastructure", RuleMode.ForbidDependency)]
[assembly: Dependency(
    "orders-handler-to-repository",
    "SubmitOrderHandler",
    "IOrderRepository",
    BrickDependencyKinds.TypeReference)]

[assembly: Policy("")]
[assembly: Rule("", "", "")]
[assembly: Dependency("", "", "", "")]
