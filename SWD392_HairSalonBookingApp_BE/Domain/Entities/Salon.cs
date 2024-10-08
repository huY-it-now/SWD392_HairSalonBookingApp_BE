﻿namespace Domain.Entities
{
    public class Salon : BaseEntity
    {
        public string Address { get; set; }

        #region RelationShip
        public ICollection<Category> Categories { get; set; }
        public ICollection<SalonWithMembers> SalonWithMembers { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
