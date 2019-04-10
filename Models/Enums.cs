
using System.ComponentModel.DataAnnotations;

namespace Mvc4_Development_Test
{

    /// <summary>
    /// Status of an object
    /// </summary>
    public enum Status
    {
        Active = 1,
        Deleted = 2,
        Blocked = 3,
        PendingUpload = 5
    }


    /// <summary>
    /// Service of Interest
    /// </summary>
    public enum Service
    {
        [Display(Name = "Analytics")]
        Analytics = 1,

        [Display(Name = "Observe")]
        Observe = 2,

        [Display(Name = "Influence")]
        Influence = 3    
    }

    /// <summary>
    /// Title of the Customer
    /// </summary>
    public enum Titles
    {
        Dr = 1,
        Mr = 2,
        Mrs = 3,
        Ms = 4,
        Professor = 5,
        Sir = 6,
        Sister = 7       
    }
}
