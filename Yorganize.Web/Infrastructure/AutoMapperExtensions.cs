﻿using System.Linq;
using AutoMapper;

namespace Yorganize.Web.Infrastructure
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType == sourceType && x.DestinationType == destinationType);
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}