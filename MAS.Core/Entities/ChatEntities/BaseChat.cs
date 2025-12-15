using MAS.Core.Entities.Base;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAS.Core.Entities.ChatEntities
{
	public class BaseChat : BaseEntity
	{
        // navigation properties

        // many-to-one
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        // many-to-many
        [NotMapped]
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}
