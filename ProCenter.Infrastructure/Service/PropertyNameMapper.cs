using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Infrastructure.Service
{
    using System.Linq.Expressions;
    using System.Reflection;
    using AutoMapper;
    using Pillar.Common.Utility;

    /// <summary>
    /// Static class for getting property name mapping information from AutoMapper.
    /// </summary>
    public static class PropertyNameMapper
    {
        #region Public Methods

        /// <summary>
        /// Gets the destination property names.
        /// </summary>
        /// <param name="sourceType">
        /// Type of the source. 
        /// </param>
        /// <param name="destinationType">
        /// Type of the destination. 
        /// </param>
        /// <param name="sourcePropertyNames">
        /// The source property names. 
        /// </param>
        /// <returns>
        /// List of destination property names. 
        /// </returns>
        public static string[] GetDestinationPropertyNames(Type sourceType, Type destinationType, params string[] sourcePropertyNames)
        {
            var destinationPropertyNames = new List<string>();

            var typeMap = Mapper.FindTypeMapFor(sourceType, destinationType);

            if (typeMap != null)
            {
                foreach (var propertyMap in typeMap.GetPropertyMaps().Where(m => m.IsMapped()))
                {
                    var useDestinationName = false;
                    if (propertyMap.CustomExpression != null)
                    {
                        var propertyChainMembers = GetPropertyChainMembers(propertyMap.CustomExpression).Skip(1);
                        if (propertyChainMembers.Any(sourcePropertyNames.Contains))
                        {
                            useDestinationName = true;
                        }
                    }
                    else if (propertyMap.SourceMember != null)
                    {
                        if (sourcePropertyNames.Contains(propertyMap.SourceMember.Name))
                        {
                            useDestinationName = true;
                        }
                    }

                    if (useDestinationName)
                    {
                        destinationPropertyNames.Add(propertyMap.DestinationProperty.Name);
                    }
                }
            }

            return destinationPropertyNames.ToArray();
        }

        /// <summary>
        /// Gets the name of the distinct destination property.
        /// </summary>
        /// <param name="sourceType">
        /// Type of the source. 
        /// </param>
        /// <param name="destinationType">
        /// Type of the destination. 
        /// </param>
        /// <param name="sourcePropertyName">
        /// Name of the source property. 
        /// </param>
        /// <returns>
        /// Null if no property found, otherwise destination property name. 
        /// </returns>
        public static string GetDistinctDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName)
        {
            string destinationPropertyName = null;

            var typeMap = Mapper.FindTypeMapFor(sourceType, destinationType);

            if (typeMap != null)
            {
                foreach (var propertyMap in typeMap.GetPropertyMaps().Where(m => m.IsMapped()))
                {
                    var useDestinationName = false;
                    if (propertyMap.CustomExpression != null)
                    {
                        var propertyChainMembers = GetPropertyChainMembers(propertyMap.CustomExpression).Skip(1).ToList();

                        if (propertyChainMembers.Count > 0 && propertyChainMembers[propertyChainMembers.Count - 1] == sourcePropertyName)
                        {
                            useDestinationName = true;
                        }
                    }
                    else if (propertyMap.SourceMember != null)
                    {
                        if (sourcePropertyName == propertyMap.SourceMember.Name)
                        {
                            useDestinationName = true;
                        }
                    }

                    if (useDestinationName)
                    {
                        destinationPropertyName = propertyMap.DestinationProperty.Name;
                    }
                }
            }

            return destinationPropertyName;
        }

        /// <summary>
        /// Gets the source property chain.
        /// </summary>
        /// <param name="destinationType">
        /// Type of the destination. 
        /// </param>
        /// <param name="destinationPropertyName">
        /// Name of the destination property. 
        /// </param>
        /// <returns>
        /// List of memberInfos of source. 
        /// </returns>
        public static IEnumerable<MemberInfo> GetSourcePropertyChain(Type destinationType, string destinationPropertyName)
        {
            var memberInfoList = new List<MemberInfo>();

            var typeMap = Mapper.GetAllTypeMaps().FirstOrDefault(t => t.DestinationType == destinationType);

            if (typeMap != null)
            {
                var propertyMaps = typeMap.GetPropertyMaps().Where(m => m.IsMapped());
                foreach (var propertyMap in propertyMaps)
                {
                    if (propertyMap.DestinationProperty.Name == destinationPropertyName)
                    {
                        if (propertyMap.CustomExpression != null)
                        {
                            memberInfoList.AddRange(GetPropertyChainMemberInfos(propertyMap.CustomExpression).Reverse());
                        }
                        else if (propertyMap.SourceMember != null)
                        {
                            memberInfoList.Add(propertyMap.SourceMember);
                        }
                    }
                }
            }

            return memberInfoList;
        }

        #endregion

        #region Methods

        private static IEnumerable<MemberInfo> GetPropertyChainMemberInfos(LambdaExpression expr)
        {
            var me = ExpressionTreeWalker.FindFirst<MemberExpression>(expr);

            while (me != null)
            {
                yield return me.Member;

                me = me.Expression as MemberExpression;
            }
        }

        private static IEnumerable<string> GetPropertyChainMembers(LambdaExpression expr)
        {
            var me = ExpressionTreeWalker.FindFirst<MemberExpression>(expr);

            while (me != null)
            {
                string propertyName = me.Member.Name;

                yield return propertyName;

                me = me.Expression as MemberExpression;
            }
        }

        #endregion
    }
}
