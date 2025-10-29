using RecruitingChallenge.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Tests.Utilities.Builders.Entities
{
    public class TestProductEntityBuilder
    {
        private readonly ProductEntity productEntity = new();

        public TestProductEntityBuilder WithId(Guid guid)
        {
            productEntity.Id = guid;
            return this;
        }

        public TestProductEntityBuilder WithUnitPrice(decimal unitPrice)
        {
            productEntity.UnitPrice = unitPrice;
            return this;
        }

        public TestProductEntityBuilder WithEntryDate(DateTime entryDate)
        {
            productEntity.EntryDate = entryDate;
            return this;
        }

        public TestProductEntityBuilder WithDescription(string description)
        {
            productEntity.Description = description;
            return this;
        }

        public TestProductEntityBuilder WithName(string name)
        {
            productEntity.Name = name;
            return this;
        }

        public ProductEntity Build() => productEntity;
    }
}
