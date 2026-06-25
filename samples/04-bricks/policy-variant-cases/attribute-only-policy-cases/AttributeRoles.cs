using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public static class AttributeRoles
{
    public const string DomainAggregate = "Attr.Domain.Aggregate";
    public const string ApplicationHandler = "Attr.Application.Handler";
    public const string RepositoryContract = "Attr.Application.RepositoryContract";
    public const string ValueObject = "Attr.Domain.ValueObject";
    public const string InfrastructureAdapter = "Attr.Infrastructure.Adapter";
    public const string PaymentNamespace = "Attr.Namespace.PaymentApplication";
    public const string ExternalPaymentGateway = "Attr.External.PaymentGateway";
}
