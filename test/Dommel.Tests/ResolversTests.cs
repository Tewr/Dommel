﻿using System.ComponentModel.DataAnnotations.Schema;
using Xunit;
using static Dommel.DommelMapper;

namespace Dommel.Tests
{
    public class ResolversTests
    {
        private static readonly ISqlBuilder _sqlBuilder = new SqlServerSqlBuilder();

        [Fact]
        public void Table_NoCacheConflictNestedClass()
        {
            Assert.Equal("[BarA]", Resolvers.Table(typeof(Foo.Bar), _sqlBuilder));
            Assert.Equal("[BarB]", Resolvers.Table(typeof(Baz.Bar), _sqlBuilder));
        }

        [Fact]
        public void Column_NoCacheConflictNestedClass()
        {
            Assert.Equal("[BazA]", Resolvers.Column(typeof(Foo.Bar).GetProperty("Baz"), _sqlBuilder));
            Assert.Equal("[BazB]", Resolvers.Column(typeof(Baz.Bar).GetProperty("Baz"), _sqlBuilder));
        }

        [Fact]
        public void ForeignKey_NoCacheConflictNestedClass()
        {
            var foreignKeyA = Resolvers.ForeignKeyProperty(typeof(Foo.BarChild), typeof(Foo.Bar), out _);
            var foreignKeyB = Resolvers.ForeignKeyProperty(typeof(Baz.BarChild), typeof(Baz.Bar), out _);

            Assert.Equal(typeof(Foo.BarChild).GetProperty("BarId"), foreignKeyA);
            Assert.Equal(typeof(Baz.BarChild).GetProperty("BarId"), foreignKeyB);
        }

        public class Foo
        {
            [Table("BarA")]
            public class Bar
            {
                [Column("BazA")]
                public string Baz { get; set; }
            }

            [Table("BarA")]
            public class BarChild
            {
                public int BarId { get; set; }
            }
        }

        public class Baz
        {
            [Table("BarB")]
            public class Bar
            {
                [Column("BazB")]
                public string Baz { get; set; }
            }

            [Table("BarA")]
            public class BarChild
            {
                public int BarId { get; set; }
            }
        }
    }
}
