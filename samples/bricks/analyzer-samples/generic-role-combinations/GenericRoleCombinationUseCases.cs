using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.GenericRoleCombinations;

[UserInterfaceEndpoint]
public sealed class AccountSettingsPage
{
    private readonly CustomerDatabaseAdapter _database = default!;
}

[DatabaseAdapter]
public sealed class CustomerDatabaseAdapter
{
}

[Role(GenericRoleNames.BusinessWorkflow)]
public sealed class SubscriptionRenewalWorkflow
{
    private readonly PaymentVendorSdkAdapter _vendorSdk = default!;
}

[VendorSdkAdapter]
public sealed class PaymentVendorSdkAdapter
{
}

[CheckoutFeatureInternalApi]
public sealed class CheckoutDiscountCalculator
{
    private readonly BillingInvoiceNumberGenerator _billingInternalApi = default!;
}

[BillingFeatureInternalApi]
public sealed class BillingInvoiceNumberGenerator
{
}

[Role(GenericRoleNames.ProductionRuntimeComponent)]
public sealed class PaymentSettlementWorker
{
    private readonly PaymentSettlementTestHarness _testHarness = default!;
}

[Role(GenericRoleNames.TestHarness)]
public sealed class PaymentSettlementTestHarness
{
}

[Role(GenericRoleNames.GeneratedClient)]
public sealed class PartnerOrdersGeneratedClient
{
    private readonly PartnerDiscountPolicy _businessPolicy = default!;
}

[Role(GenericRoleNames.BusinessPolicy)]
public sealed class PartnerDiscountPolicy
{
}

[Role(GenericRoleNames.ScheduledJob)]
public sealed class NightlyAccountExportJob
{
    public void Run()
    {
    }
}

[Role(GenericRoleNames.CheckpointStore)]
public interface IAccountExportCheckpointStore
{
    void SaveCheckpoint(string checkpoint);
}
