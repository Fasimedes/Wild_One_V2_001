﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class Trader : LivingEntity
    {
        public int ID { get; }
        public Trader(int id, string name) : base(name, 9999, 9999, new List<PlayerAttribute>(), 9999)
        {
            ID = id;
        }
    }
}
