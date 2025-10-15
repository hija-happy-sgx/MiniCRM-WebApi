namespace CRMWepApi.Enums
{
    public enum  UserRole
    {
        Admin,
        Manager,
        SalesRepManager,
        SalesRep
    }

    public enum LeadStatus
    {
        New, 
        Contacted,
        Qualified,
        Converted,
        Disqualified
    }

    public enum DealStatus
    {
      Open,
      Won,
      Lost


    }

    public enum PipelineStages
    {
               InitialContact = 1,
        NeedsAnalysis = 2,
        ProposalSent = 3,
        Negotiation = 4,
        ClosedWon = 5,
        ClosedLost = 6
    }
}
