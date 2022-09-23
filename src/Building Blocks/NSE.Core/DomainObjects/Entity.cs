using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;
            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;
            return Id.Equals(compareTo.Id);
        }
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 159) + Id.GetHashCode();
        }

    }
}
