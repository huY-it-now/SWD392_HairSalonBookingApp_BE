﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Stylist
{
    public class RegisterWorkScheduleDTO
    {
        public Guid StylistId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public List<WorkShiftDTO> WorkShifts {  get; set; }
    }
}