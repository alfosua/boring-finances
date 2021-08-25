using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoringSoftware.Finances.Entities.FinancialUnits
{
    public class FinancialUnitType
    {
        #region Mapped Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, StringLength(8)]
        public string Code { get; set; }
        
        #endregion Mapped Properties

        #region Navigation Properties

        public ICollection<FinancialUnit> FinanciaUnits { get; set; }

        #endregion Navigation Properties

        #region Helper Properties

        public FinancialUnitTypeOption Option => (FinancialUnitTypeOption)Id;

        #endregion Helper Properties
    }
}
