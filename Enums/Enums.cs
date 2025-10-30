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
        Converted,
        Qualified,
        Disqualified
    }

    public enum DealStatus
    {
      Open,
      Contacted,
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
