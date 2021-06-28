using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace PeD.Data
{
    public static class ContextExtension
    {
        public static PropertyBuilder<T> JsonConversion<T>(this PropertyBuilder<T> pb, ValueComparer<T> comparer)
        {
            pb.HasConversion(
                    jt => JsonConvert.SerializeObject(jt),
                    str => JsonConvert.DeserializeObject<T>(str))
                .Metadata.SetValueComparer(comparer);
            return pb;
        }

        public static PropertyBuilder<List<T>> JsonConversion<T>(this PropertyBuilder<List<T>> pb)
        {
            var comparer = new ValueComparer<List<int>>(
                (l1, l2) => l1.SequenceEqual(l2),
                l => l.Aggregate(0, (i, s) => HashCode.Combine(i, s.GetHashCode())));
            pb.HasConversion(
                    jt => JsonConvert.SerializeObject(jt),
                    str => JsonConvert.DeserializeObject<List<T>>(str))
                .Metadata.SetValueComparer(comparer);
            return pb;
        }
    }
}