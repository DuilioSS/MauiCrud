using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauicrud.flussodati
{
    public class Lavoratoreposta : ValueChangedMessage<Lavoratoremessaggio>
    {
        public Lavoratoreposta(Lavoratoremessaggio value) : base(value)
        {


        }
    }
}
