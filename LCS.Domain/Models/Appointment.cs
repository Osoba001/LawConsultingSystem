﻿using LCS.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCS.Domain.Models
{
    public class Appointment: ModelBase
    {
        public Appointment(Lawyer lawyer, Client client, DateTime reviewDate, double charge, AppointmentType appointmentType)
        {
            Lawyer = lawyer;
            Client = client;
            AppointmentType = appointmentType;
            ReviewDate = reviewDate;
            Charge = charge;
        }
        public Lawyer Lawyer { get; set; }
        public Client Client { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool HasReviewed { get; set; }
        public bool IsCancel { get; set; }
        public double Charge { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}
