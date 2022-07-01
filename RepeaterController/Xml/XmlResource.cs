/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace RepeaterController.Xml
{
    /// <remarks>
    /// This class is used to denote which classes in an assembly are particle modifiers.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class XmlImportExportAttribute : Attribute
    {
        /**
         * Fields
         */
        private readonly string nodeName;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the XML node name
        /// </summary>
        public string NodeName
        {
            get { return nodeName; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlImportExportAttribute"/>.
        /// </summary>
        public XmlImportExportAttribute()
        {
            this.nodeName = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlImportExportAttribute"/>.
        /// </summary>
        /// <param name="name"></param>
        public XmlImportExportAttribute(string name)
        {
            this.nodeName = name;
        }
    } // public class XmlImportExportAttribute : Attribute

    /// <summary>
    /// Helper class to load resource data from disk.
    /// </summary>
    public class XmlResource
    {
        /**
         * Fields
         */
        /// <summary>
        /// XML root element.
        /// </summary>
        public const string DEFAULT_NAMESPACE = "RCResource";

        private const string XML_VERSION = "1.0";

        private const string DIGEST_ATTRIBUTE = "digest";
        private const string VERSION_ATTRIBUTE = "version";

        private string sourcePath;
        private string sourceFile;

        private XmlWriterSettings xmlWriterSettings;

        private XmlDocument parent;
        private XmlElement parentRoot;
        private string parentNodeName;

        private XmlDocument rsrc;
        private XmlElement root;

        private bool failedDigest = false;
        private bool ignoreDigest = false;
        private bool isCreatedRoot = false;

        /**
         * Properties
         */
        /// <summary>
        /// Gets a value from the named parent.
        /// </summary>
        public object this[string valueName]
        {
            get { return Get<object>(valueName); }
        }

        /// <summary>
        /// Returns the XML source path this resource was created from.
        /// </summary>
        public string XmlSourcePath
        {
            get { return sourcePath; }
        }

        /// <summary>
        /// Returns the XML source file this resource was created from.
        /// </summary>
        public string XmlSourceFile
        {
            get { return sourceFile; }
        }

        /// <summary>
        /// Gets the raw XML docuemnt for this resource.
        /// </summary>
        public XmlDocument Document
        {
            get { return rsrc; }
        }

        /// <summary>
        /// Flag indicating whether this XML data has failed the digest check.
        /// </summary>
        public bool FailedDigest
        {
            get { return failedDigest; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        /// <param name="ignoreDigest"></param>
        public XmlResource(bool ignoreDigest = false)
        {
            this.xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.OmitXmlDeclaration = true;
            xmlWriterSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "  ";
            xmlWriterSettings.Encoding = Encoding.UTF8;

            this.sourcePath = string.Empty;
            this.sourceFile = string.Empty;

            this.parent = null;
            this.parentRoot = null;
            this.parentNodeName = string.Empty;

            this.ignoreDigest = ignoreDigest;
            this.isCreatedRoot = false;

            CreateNewXml();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ignoreDigest"></param>
        public XmlResource(string fileName, bool ignoreDigest = false) : this(ignoreDigest)
        {
            this.sourcePath = fileName;
            this.sourceFile = Path.GetFileName(fileName);
            LoadXml(fileName);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        /// <param name="element"></param>
        private XmlResource(XmlNode element) : this()
        {
            XmlNode local = this.rsrc.ImportNode(element, true);
            root.AppendChild(local);
            isCreatedRoot = true;
        }

        /// <summary>
        /// Creates a new <see cref="XmlResource"/> that is a child of this <see cref="XmlResource"/>. First node created is the "root" of the resource.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlResource CreateNode(string name)
        {
            if (!isCreatedRoot)
                isCreatedRoot = true;

            XmlElement element = rsrc.CreateElement(name);
            XmlElement parentRootElement = null;
            if (parent != null)
                parentRootElement = parent.SelectSingleNode(string.Format("descendant::{0}", this.parentNodeName)) as XmlElement;

            if (!HasNode(name))
            {
                if (parent != null)
                {
                    element.SetAttribute("subtree", "true");

                    XmlNode local = parent.ImportNode(element, true);
                    parentRootElement.AppendChild(local);
                }
                else
                    this.root.AppendChild(element);
            }

            // generate a new node
            XmlResource remote = new XmlResource(element);
            if (parent != null)
            {
                remote.parent = parent;
                remote.parentRoot = parentRootElement;
            }
            else
            {
                remote.parent = this.rsrc;
                remote.parentRoot = this.root;
            }
            remote.parentNodeName = name;
            remote.isCreatedRoot = true;

            return remote;
        }

        /// <summary>
        /// Helper to determine if the named node exists in the schema.
        /// </summary>
        /// <param name="name">Name or XPath Query</param>
        /// <returns></returns>
        public bool HasNode(string name)
        {
            if (!isCreatedRoot)
                return false;

            XmlElement parent = rsrc.SelectSingleNode(string.Format("descendant::{0}", name)) as XmlElement;
            if (parent != null)
                return true;
            return false;
        }

        /// <summary>
        /// Gets data elements by reflection of fields and properties with the <see cref="XmlImportExportAttribute"/> of the passed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void GetByReflection<T>(T value)
        {
            if (!isCreatedRoot)
                throw new InvalidOperationException("No initial root element has been created!");

            Type valueType = typeof(T);

            // look for the XmlImportExport attribute on fields
            foreach (FieldInfo field in valueType.GetFields())
            {
                foreach (XmlImportExportAttribute attr in field.GetCustomAttributes<XmlImportExportAttribute>(true))
                {
                    string nodeName = field.Name;
                    string fieldName = nodeName;
                    if (attr.NodeName != string.Empty)
                        nodeName = attr.NodeName;

                    object fieldValue = this.Get<object>(nodeName);
                    field.SetValue(value, fieldValue);
                }
            }

            // look for the XmlImportExport attribute on properties
            foreach (PropertyInfo property in valueType.GetProperties())
            {
                foreach (XmlImportExportAttribute attr in property.GetCustomAttributes<XmlImportExportAttribute>(true))
                {
                    string nodeName = property.Name;
                    string propertyName = nodeName;
                    if (attr.NodeName != string.Empty)
                        nodeName = attr.NodeName;

                    object propertyValue = this.Get<object>(nodeName);
                    property.SetValue(value, propertyValue);
                }
            }
        }

        /// <summary>
        /// Gets a data element by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public T Get<T>(string valueName)
        {
            if (!isCreatedRoot)
                return default(T);

            if (HasNode(valueName))
            {
                XmlElement element = rsrc.SelectSingleNode(string.Format("descendant::{0}", valueName)) as XmlElement;
                if (element == null)
                    return default(T);

                object ret = null;
                if (element.HasAttribute("subtree"))
                    ret = new XmlResource(element);
                else
                {
                    // special case ... this should REALLY be handled properly with "subtree" attribute
                    if (typeof(T) == typeof(XmlResource))
                    {
                        ret = new XmlResource(element);
                        return (T)ret;
                    }

                    // make sure we have the type attribute
                    if (!element.HasAttribute("type"))
                        throw new InvalidOperationException(string.Format("Cannot load Xml data that is missing type! {0}", element.Name));

                    // deserialize the XML element to a type
                    Type valueType = Type.GetType(element.Attributes["type"].InnerText);
                    XmlSerializer serializer = new XmlSerializer(valueType, string.Empty);
                    using (StringReader strReader = new StringReader(element.InnerXml))
                    {
                        ret = serializer.Deserialize(strReader);
                        if (element.HasAttribute("aqn"))
                        {
                            Type originalType = null;
                            HandleToDictionary(ref valueType, out originalType, ref ret);
                            // ?? perhaps handle the boolean result?
                        }
                    }
                }

                // make sure that we're getting the type we expect
                if (!(ret is T) && !(typeof(T) is object) && ret != null)
                    throw new InvalidCastException(string.Format("Cannot cast type {0} to {1}. Expected wrong type?", typeof(T).FullName, ret.GetType().FullName));

                return (T)ret;
            }
            else
                return default(T);
        }

        /// <summary>
        /// Adds data elements by reflection of fields and properties with the <see cref="XmlImportExportAttribute"/> of the passed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SubmitByReflection<T>(T value)
        {
            if (!isCreatedRoot)
                throw new InvalidOperationException("No initial root element has been created!");

            Type valueType = typeof(T);

            // look for the XmlImportExport attribute on fields
            foreach (FieldInfo field in valueType.GetFields())
            {
                foreach (XmlImportExportAttribute attr in field.GetCustomAttributes<XmlImportExportAttribute>(true))
                {
                    string nodeName = field.Name;
                    object fieldValue = field.GetValue(value);
                    if (attr.NodeName != string.Empty)
                        nodeName = attr.NodeName;
                    this.Submit(nodeName, fieldValue, field.FieldType, field.Name);
                }
            }

            // look for the XmlImportExport attribute on properties
            foreach (PropertyInfo property in valueType.GetProperties())
            {
                foreach (XmlImportExportAttribute attr in property.GetCustomAttributes<XmlImportExportAttribute>(true))
                {
                    string nodeName = property.Name;
                    object propertyValue = property.GetValue(value);
                    if (attr.NodeName != string.Empty)
                        nodeName = attr.NodeName;
                    this.Submit(nodeName, propertyValue, property.PropertyType, property.Name);
                }
            }
        }

        /// <summary>
        /// Adds a data element to the <see cref="XmlResource"/>.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <param name="fieldName"></param>
        public void Submit<T>(string valueName, T value, Type valueType = null, string fieldName = null)
        {
            if (!isCreatedRoot)
                throw new InvalidOperationException("No initial root element has been created!");

            if (valueType == null)
                valueType = typeof(T);

            XmlElement element = rsrc.CreateElement(valueName);
            object obj = value;

            // we have to do some mangling of the system dictionary type because its unsupported
            Type originalType = null;
            if (HandleFromDictionary(ref valueType, out originalType, ref obj))
                element.SetAttribute("aqn", originalType.AssemblyQualifiedName);

            string dataType = valueType.FullName;
            if (!dataType.StartsWith("System."))
                dataType = valueType.AssemblyQualifiedName;
            element.SetAttribute("type", dataType);
            if ((fieldName != null) && (valueName != fieldName))
                element.SetAttribute("field", fieldName);

            // serialize input object
            XmlSerializer serializer = new XmlSerializer(valueType, string.Empty);
            using (StringWriter strWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(strWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, obj);

                    // we need to read it back in so we can inject it as nodes
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreWhitespace = true;
                    XmlReader reader = XmlReader.Create(new StringReader(strWriter.ToString()), settings);

                    XmlNode inner = rsrc.ReadNode(reader);
                    element.AppendChild(inner);
                }
            }

            // append our child to the parent
            if (this.parent != null)
            {
                XmlElement current = rsrc.SelectSingleNode(string.Format("descendant::{0}", this.parentNodeName)) as XmlElement;
                current.AppendChild(element);

                XmlNode _new = parent.ImportNode(current, true);
                XmlNode old = parentRoot.SelectSingleNode(string.Format("descendant::{0}", this.parentNodeName));

                parentRoot.ReplaceChild(_new, old);
            }
            else
                root.AppendChild(element);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="originalType"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool HandleFromDictionary(ref Type paramType, out Type originalType, ref object obj)
        {
            originalType = paramType;

            if (paramType.GetInterface(typeof(IDictionary).Name) != null)
            {
                if (paramType.IsConstructedGenericType)
                {
                    // do we have more then 2 generics? (should not happen)
                    if (paramType.GenericTypeArguments.Length > 2)
                        throw new InvalidOperationException("Shouldn't have more then 2 generic type arguments .. but we do");

                    Type key = paramType.GenericTypeArguments[0];
                    Type value = paramType.GenericTypeArguments[1];
                    Type dictType = typeof(XmlDictionarySerializable<,>).MakeGenericType(new Type[] { key, value });

                    var instance = Activator.CreateInstance(dictType);
                    MethodInfo fromMethod = instance.GetType().GetMethod("FromDictionary");
                    instance = fromMethod.Invoke(instance, new object[] { obj });

                    obj = null;
                    obj = instance;

                    paramType = dictType;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="originalType"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool HandleToDictionary(ref Type paramType, out Type originalType, ref object obj)
        {
            originalType = paramType;

            // we have to do some mangling of the system dictionary type because its unsupported
            if (paramType.GetInterface(typeof(IDictionary).Name) != null)
            {
                if (paramType.IsConstructedGenericType)
                {
                    // do we have more then 2 generics? (should not happen)
                    if (paramType.GenericTypeArguments.Length > 2)
                        throw new InvalidOperationException("Shouldn't have more then 2 generic type arguments .. but we do");

                    Type key = paramType.GenericTypeArguments[0];
                    Type value = paramType.GenericTypeArguments[1];
                    Type dictType = typeof(Dictionary<,>).MakeGenericType(new Type[] { key, value });

                    var instance = Activator.CreateInstance(dictType);
                    MethodInfo toMethod = obj.GetType().GetMethod("ToDictionary");
                    instance = toMethod.Invoke(instance, new object[] { obj });

                    obj = null;
                    obj = instance;

                    paramType = dictType;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Internal helper to generate the blank XML schema.
        /// </summary>
        private void CreateNewXml()
        {
            this.rsrc = new XmlDocument();
            rsrc.AppendChild(rsrc.CreateComment(" DO NOT EDIT THIS FILE "));

            this.root = rsrc.CreateElement(DEFAULT_NAMESPACE);
            this.root.SetAttribute(VERSION_ATTRIBUTE, XML_VERSION);
            rsrc.AppendChild(root);
        }

        /// <summary>
        /// Load XML data from disk.
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadXml(string fileName)
        {
            Assembly thisAssembly = Assembly.GetAssembly(typeof(XmlResource));

            try
            {
                Stream onDiskData = null;
                if (File.Exists(fileName))
                    onDiskData = File.Open(fileName, FileMode.Open, FileAccess.Read);
                else
                {
                    string directoryPath = Path.GetDirectoryName(fileName);
                    directoryPath = directoryPath.Replace(Path.DirectorySeparatorChar, '.');
                    if (!directoryPath.EndsWith("."))
                        directoryPath += ".";
                    if (directoryPath == ".")
                        directoryPath = string.Empty;

                    // bryanb: so it seems the fucking god damn .NET vs Mono is different in this regard...
                    // .NET will replace - with _ and Mono leaves them as-is when embedding files into the
                    // executable assembly
                    bool runningOnMono = Type.GetType("Mono.Runtime") != null;
                    if (!runningOnMono && directoryPath.Contains("-"))
                        directoryPath = directoryPath.Replace('-', '_');

                    fileName = Path.GetFileName(fileName);

                    string assemblyPath = thisAssembly.GetName().Name + ".Resource." + directoryPath + fileName;
                    List<string> embedTest = new List<string>(thisAssembly.GetManifestResourceNames());
                    if (!embedTest.Contains(assemblyPath))
                    {
                        Messages.WriteWarning("unable to locate resource [" + directoryPath + "], assemblyPath = " + assemblyPath);
                        return;
                    }

                    onDiskData = thisAssembly.GetManifestResourceStream(assemblyPath);
                    ignoreDigest = true;    // always ignore digests on internal resources
                }

                if (onDiskData != null)
                {
                    if (onDiskData.CanRead)
                    {
                        // reset read position
                        onDiskData.Position = 0;

                        StreamReader reader = new StreamReader(onDiskData);
                        rsrc.LoadXml(reader.ReadToEnd());
                        root = rsrc.SelectSingleNode(string.Format("descendant::{0}", DEFAULT_NAMESPACE)) as XmlElement;
                        if (root == null)
                        {
                            isCreatedRoot = false;
                            return;
                        }

                        // do we have a proper version attribute?
                        if (root.HasAttribute(VERSION_ATTRIBUTE))
                        {
                            if (root.GetAttribute(VERSION_ATTRIBUTE) != XML_VERSION)
                            {
                                onDiskData.Close();
                                throw new InvalidDataException(string.Format("Xml version does not match! {0} != {1}", root.GetAttribute(VERSION_ATTRIBUTE), XML_VERSION));
                            }
                        }
                        else
                        {
                            onDiskData.Close();
                            throw new InvalidDataException("Xml does not define a version attribute!");
                        }

                        // do we have a proper digest attribute?
                        if (root.HasAttribute(DIGEST_ATTRIBUTE))
                        {
                            SHA512Managed sha = new SHA512Managed();
                            byte[] storedHash = Convert.FromBase64String(root.GetAttribute(DIGEST_ATTRIBUTE));
                            root.RemoveAttribute(DIGEST_ATTRIBUTE);
                            byte[] computedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(rsrc.OuterXml));

                            if (computedHash.Length == storedHash.Length)
                            {
                                int i = 0;
                                while ((i < computedHash.Length) && (computedHash[i] == storedHash[i]))
                                    i++;

                                if (i != computedHash.Length)
                                {
                                    failedDigest = true;
                                    if (ignoreDigest)
                                    {
                                        // since the digest doesn't match, lets just regenerate it
                                        GenerateDigest();
                                    }
                                    else
                                    {
                                        onDiskData.Close();
                                        throw new InvalidDataException("Xml digest did not match.");
                                    }
                                }
                            }
                            else
                            {
                                failedDigest = true;
                                if (ignoreDigest)
                                {
                                    // since the digest doesn't match, lets just regenerate it
                                    GenerateDigest();
                                }
                                else
                                {
                                    onDiskData.Close();
                                    throw new InvalidDataException("Xml digest did not match.");
                                }
                            }
                        }
                        else
                        {
                            failedDigest = true;
                            onDiskData.Close();
                            throw new InvalidDataException("Xml does not define a digest attribute!");
                        }


                        isCreatedRoot = true;
                    }
                    onDiskData.Close();
                }
            }
            catch (FileNotFoundException) { }
        }

        /// <summary>
        /// Generate the digest hash for the XML data.
        /// </summary>
        private void GenerateDigest()
        {
            if (root.HasAttribute(DIGEST_ATTRIBUTE))
                root.RemoveAttribute(DIGEST_ATTRIBUTE);

            // hash the data
            SHA512Managed sha = new SHA512Managed();
            string hashStr = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(rsrc.OuterXml)));

            root.SetAttribute(DIGEST_ATTRIBUTE, hashStr);
        }

        /// <summary>
        /// Saves XML data to disk.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveXml(string fileName)
        {
            if (!isCreatedRoot)
                throw new InvalidOperationException("No initial root element has been created!");

            // begin writing data
            FileStream onDiskData = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            if (onDiskData != null)
            {
                TextWriter writer = new StreamWriter(onDiskData);

                StringWriter strWriter = new StringWriter();
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.OmitXmlDeclaration = false;
                writerSettings.ConformanceLevel = ConformanceLevel.Auto;
                writerSettings.Indent = true;
                writerSettings.IndentChars = xmlWriterSettings.IndentChars;
                writerSettings.Encoding = Encoding.UTF8;

                // write XML
                XmlWriter xmlWriter = XmlWriter.Create(strWriter, xmlWriterSettings);
                GenerateDigest();

                rsrc.Save(xmlWriter);
                writer.WriteLine(strWriter.ToString());

                xmlWriter.Close();
                strWriter.Flush();
                strWriter.Close();

                writer.Flush();

                onDiskData.Flush();
                onDiskData.Close();
            }
        }
    } // public class XmlResource
} // namespace BoilerEngine.Xml
