using CRMWepApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CRMWebApi.DTOs
{
    public class DealDto
    {
        public int DealId { get; set; }
        public int LeadId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public Deal Status { get; set; } // Enum
        public int StageId { get; set; }
    }

    //public class CreateDealDto
    //{
    //    public int LeadId { get; set; }
    //    public string Title { get; set; }
    //    public decimal Amount { get; set; }
    //}

    public class CreateDealDto
    {
        
       
        public int LeadId { get; set; }
   
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int StageId { get; set; } 
        public int Probablity { get; set; }
        public DateTime ExpectedCloseDate { get; set; } 
    }


}
