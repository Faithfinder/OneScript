﻿using System;
using System.Diagnostics.Contracts;
using System.Xml.Schema;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace ScriptEngine.HostedScript.Library.XMLSchema
{
    [ContextClass("АннотацияXS", "XSAnnotation")]
    public class XSAnnotation : AutoContext<XSAnnotation>, IXSComponent
    {
        internal readonly XmlSchemaAnnotation InternalObject;

        private XSAnnotation()
        {
            InternalObject = new XmlSchemaAnnotation();

            Content = new XSComponentList();
            Content.Cleared += Content_Cleared;
            Content.Inserted += Content_Inserted;

            Components = new XSComponentFixedList();
        }

        #region OneScript

        #region Properties

        [ContextProperty("Аннотация", "Annotation")]
        public XSAnnotation Annotation => null;

        [ContextProperty("Компоненты", "Components")]
        public XSComponentFixedList Components { get; }

        [ContextProperty("Контейнер", "Container")]
        public IXSComponent Container { get; private set; }

        [ContextProperty("КорневойКонтейнер", "RootContainer")]
        public IXSComponent RootContainer { get; private set; }

        [ContextProperty("Схема", "Schema")]
        public XMLSchema Schema => RootContainer.Schema;

        [ContextProperty("ТипКомпоненты", "ComponentType")]
        public XSComponentType ComponentType => XSComponentType.Annotation;

        [ContextProperty("Состав", "Content")]
        public XSComponentList Content { get; }

        #endregion

        #region Methods

        [ContextMethod("КлонироватьКомпоненту", "CloneComponent")]
        public IXSComponent CloneComponent(bool recursive = true) => throw new NotImplementedException();

        [ContextMethod("ОбновитьЭлементDOM", "UpdateDOMElement")]
        public void UpdateDOMElement() => throw new NotImplementedException();

        [ContextMethod("Содержит", "Contains")]
        public bool Contains(IXSComponent component) => Components.Contains(component);

        #region Constructors

        [ScriptConstructor(Name = "По умолчанию")]
        public static XSAnnotation Constructor() => new XSAnnotation();

        #endregion

        #endregion

        #endregion

        #region IXSComponent

        XmlSchemaObject IXSComponent.SchemaObject => InternalObject;

        void IXSComponent.BindToContainer(IXSComponent rootContainer, IXSComponent container)
        {
            RootContainer = rootContainer;
            Container = container;
        }

        #endregion

        #region XSComponentListEvents

        private void Content_Inserted(object sender, XSComponentListEventArgs e)
        {
            var component = e.Component;

            if (!(component is IXSAnnotationItem))
                throw RuntimeException.InvalidArgumentType();

            component.BindToContainer(RootContainer, this);
            Components.Add(component);
            InternalObject.Items.Add(component.SchemaObject);
        }

        private void Content_Cleared(object sender, EventArgs e)
        {
            Components.Clear();
            InternalObject.Items.Clear();
        }
        
        #endregion
    }
}