﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using SheepAspect.Commons.Observations;
using SheepAspect.Framework;
using SheepAspect.UnitTest.TestHelper;
using FluentAssertions.EventMonitoring;

namespace SheepAspect.Commons.Test.NotifyPropertiesTests
{
    [TestFixture]
    public class NotifyPropertiesTest
    {
        private dynamic target;

        [TestFixtureSetUp]
        public void Weave()
        {
            var provider = new AttributiveAspectProvider();
            var aspect = provider.GetDefinition(typeof (TestNotifyPropertiesAspect));
            target = WeaveTestHelper.WeaveAndLoadType(GetType(), aspect, typeof(NotifyPropertiesSut));
            ((object)target).MonitorEvents();
        }

        [Assert]
        public void Change1Property()
        {
            var read = target.One;

            target.One = "change";
            ShouldRaisePropertyChangeFor(x=> x.One);
        }

        private void ShouldRaisePropertyChangeFor(Expression<Func<NotifyPropertiesSut, object>> sutExpression)
        {
            var mExp = ((MemberExpression)sutExpression.Body);
            var parameter = Expression.Parameter(typeof (object), "x");
            var exp = Expression.Lambda<Func<object, object>>(
                Expression.Property(Expression.Convert(parameter, typeof(NotifyPropertiesSut)), (PropertyInfo)mExp.Member),
                parameter);
            
            ((object)target).ShouldRaisePropertyChangeFor(exp);
        }

        public class TestNotifyPropertiesAspect: NotifyPropertiesAspectBase
        {
            [SelectTypes(typeof(NotifyPropertiesSut))]
            protected override void TargetTypes(){}
        }
    }

    public class NotifyPropertiesSut: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string One { get; set; }
    }
}