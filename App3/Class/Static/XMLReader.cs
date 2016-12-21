using OKOGate.Modules;
using System;
using System.Text;
using System.Xml;
using OKOGate;
using App3.Class.Singleton;

namespace App3.Class
{
    class XMLReader
    {
        public enum PacketsTypes
        {
            ACK = 1,
            EVENT,
            ADDOBJECT,
            DELETEOBJECT,
            USERACTION,
            TESTCONNECT
        }
        public static class MessagePacker
        {
            static MessagePacker()
            {
                XMLReader.BytePacketsEnds = new byte[XMLReader.PacketsEnds.Length][];
                for (int i = 0; i < XMLReader.PacketsEnds.Length; i++)
                {
                    XMLReader.BytePacketsEnds[i] = Encoding.UTF8.GetBytes(XMLReader.PacketsEnds[i]);
                }
            }
            public static bool Pack(Message _msg)
            {
                XmlDocument xmlDocument = new XmlDocument();
                bool result;
                try
                {
                    string type = _msg.Type;
                    switch (type)
                    {
                        case "ACK_CONNECT_OKOGATE":
                            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>1</Type><DateTime></DateTime></Header><Response><Number></Number></Response></Envelope>");
                            XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                            XML.SetValue(xmlDocument, "/Envelope/Response/Number", _msg.Get("ResponseGID").ToString());
                            if (_msg.Get("Result").ToString() != "")
                            {
                                XML.SetValue(xmlDocument, "/Envelope/Response/Result", _msg.Get("Result").ToString());
                            }
                            if (_msg.Get("Comment").ToString() != "")
                            {
                                XML.SetValue(xmlDocument, "/Envelope/Response/Comment", _msg.Get("Comment").ToString());
                            }
                            _msg.Text = xmlDocument.InnerXml;
                            _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                            result = true;
                            return result;
                        case "TEST_CONNECT_OKOGATE":
                            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>6</Type><DateTime></DateTime></Header></Envelope>");
                            XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                            _msg.Text = xmlDocument.InnerXml;
                            _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                            result = true;
                            return result;
                        case "MESSAGE_AK_OKOGATE":
                        case "MESSAGE_PULT_OKOGATE":
                            {
                                int codeIndex = OKO2.GetCodeIndex(byte.Parse(_msg.Get("Class").ToString()), byte.Parse(_msg.Get("Code").ToString()));
                                if (codeIndex >= 0)
                                {
                                    xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>2</Type><DateTime></DateTime></Header><Event></Event></Envelope>");
                                    XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                                    XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                                    uint num2 = 0u;
                                    uint num3 = 0u;
                                    switch (OKO2.OKO2MsgTable[codeIndex].Type)
                                    {
                                        case OKO2.MT.MT00:
                                            result = false;
                                            return result;
                                        case OKO2.MT.MT10:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            num3 = uint.Parse(_msg.Get("Zone").ToString());
                                            break;
                                        case OKO2.MT.MT11:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            break;
                                        case OKO2.MT.MT20:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            num3 = uint.Parse(_msg.Get("Zone").ToString());
                                            break;
                                        case OKO2.MT.MT21:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            break;
                                        case OKO2.MT.MT23:
                                        case OKO2.MT.MT24:
                                            {
                                                int num4 = (OKO2.OKO2MsgTable[codeIndex].Type - OKO2.MT.MT23) * 8 + 1;
                                                uint num5 = uint.Parse(_msg.Get("Part").ToString());
                                                for (int i = 0; i < 8; i++)
                                                {
                                                    if ((num5 >> i & 1u) != 0u)
                                                    {
                                                        num2 = (uint)(i + num4);
                                                        _msg.Set("PackNext", 1);
                                                        _msg.Set("PackNextParam0", i);
                                                        break;
                                                    }
                                                }
                                                num3 = uint.Parse(_msg.Get("Zone").ToString());
                                                break;
                                            }
                                        case OKO2.MT.MT30:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            num3 = uint.Parse(_msg.Get("Zone").ToString());
                                            break;
                                        case OKO2.MT.MT40:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            num3 = uint.Parse(_msg.Get("Zone").ToString());
                                            break;
                                        case OKO2.MT.MT41:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            break;
                                        case OKO2.MT.MT50:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            num3 = uint.Parse(_msg.Get("Code").ToString()) * 256u + uint.Parse(_msg.Get("Zone").ToString());
                                            break;
                                        case OKO2.MT.MT51:
                                            num2 = uint.Parse(_msg.Get("Part").ToString());
                                            break;
                                        case OKO2.MT.MT53:
                                        case OKO2.MT.MT54:
                                        case OKO2.MT.MT55:
                                        case OKO2.MT.MT56:
                                            {
                                                int num4 = (OKO2.OKO2MsgTable[codeIndex].Type - OKO2.MT.MT53) * 8 + 1;
                                                uint num5 = uint.Parse(_msg.Get("Part").ToString());
                                                for (int i = 0; i < 8; i++)
                                                {
                                                    if ((num5 >> i & 1u) != 0u)
                                                    {
                                                        num2 = (uint)(i + num4);
                                                        _msg.Set("PackNext", 1);
                                                        _msg.Set("PackNextParam0", i);
                                                        break;
                                                    }
                                                }
                                                num3 = (uint)((byte)(int.Parse(_msg.Get("Code").ToString()) * 256 + int.Parse(_msg.Get("Zone").ToString())));
                                                break;
                                            }
                                        case OKO2.MT.MT60:
                                        case OKO2.MT.MT61:
                                        case OKO2.MT.MT62:
                                        case OKO2.MT.MT63:
                                            result = false;
                                            return result;
                                        case OKO2.MT.NTF00:
                                            result = false;
                                            return result;
                                    }
                                    if (num2 > 99u)
                                    {
                                        num2 = 0u;
                                    }
                                    if (num3 > 999u)
                                    {
                                        num3 = 0u;
                                    }
                                    XML.SetValue(xmlDocument, "/Envelope/Event/ObjectNumber", _msg.Get("Object").ToString());
                                    XML.SetValue(xmlDocument, "/Envelope/Event/DateTime", _msg.TimeStamp.ToUniversalTime().ToString("s"));
                                    XML.SetValue(xmlDocument, "/Envelope/Event/Code", OKO2.OKO2MsgTable[codeIndex].EventCode.ToString());
                                    XML.SetValue(xmlDocument, "/Envelope/Event/PartNumber", num2.ToString());
                                    XML.SetValue(xmlDocument, "/Envelope/Event/ZoneUserNumber", num3.ToString());
                                    _msg.Text = xmlDocument.InnerXml;
                                    _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                                    result = true;
                                    return result;
                                }
                                break;
                            }
                        case "ADD_OBJECT_XGUARD_OKOGATE":
                            {
                                xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>3</Type><DateTime></DateTime></Header><Object></Object></Envelope>");
                                XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                                string gUID = XMLReader.mdbconnect.GetGUID(_msg.Get("Object").ToString(), "0", "0", "0", "0");
                                XML.SetValue(xmlDocument, "/Envelope/Object/ID", gUID);
                                XML.SetValue(xmlDocument, "/Envelope/Object/Number", _msg.Get("Object").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/TypeName", _msg.Get("TypeName").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/Name", _msg.Get("Name").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/Contract", _msg.Get("Contract").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/Address", _msg.Get("Address").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/MakeDateTime", DateTime.Parse(_msg.Get("MakeDateTime").ToString()).ToUniversalTime().ToString("s"));
                                XML.SetValue(xmlDocument, "/Envelope/Object/ActivateDateTime", DateTime.Parse(_msg.Get("ActivateDateTime").ToString()).ToUniversalTime().ToString("s"));
                                XML.SetValue(xmlDocument, "/Envelope/Object/CommentForOperator", _msg.Get("CommentForOperator").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/CommentForGuard", _msg.Get("CommentForGuard").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Object/CustomersComment", _msg.Get("CustomersComment").ToString());
                                SStr fields = SStr.GetFields(_msg.Get("Parts").ToString(), ';');
                                for (int i = 0; i < fields.Count / 3; i++)
                                {
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/ID", fields.GetField(i * 3));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/Number", fields.GetField(i * 3));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/Name", fields.GetField(i * 3 + 1));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/DeviceTypeName", fields.GetField(i * 3 + 2));
                                }
                                SStr fields2 = SStr.GetFields(_msg.Get("Zones").ToString(), ';');
                                for (int i = 0; i < fields2.Count / 4; i++)
                                {
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/ID", fields2.GetField(i * 4));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/Number", fields2.GetField(i * 4));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/PartNumber", fields2.GetField(i * 4 + 1));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/Name", fields2.GetField(i * 4 + 2));
                                    XML.SetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/DeviceTypeName", fields2.GetField(i * 4 + 3));
                                }
                                _msg.Text = xmlDocument.InnerXml;
                                _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                                result = true;
                                return result;
                            }
                        case "DEL_OBJECT_XGUARD_OKOGATE":
                            {
                                xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>4</Type><DateTime></DateTime></Header><Object></Object></Envelope>");
                                XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                                XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                                string gUID = XMLReader.mdbconnect.GetGUID(_msg.Get("Object").ToString(), "0", "0", "0", "0");
                                XML.SetValue(xmlDocument, "/Envelope/Object/ID", gUID);
                                XML.SetValue(xmlDocument, "/Envelope/Object/Number", _msg.Get("Object").ToString());
                                _msg.Text = xmlDocument.InnerXml;
                                _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                                result = true;
                                return result;
                            }
                        case "USER_ACTION_XGUARD_OKOGATE":
                            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>5</Type><DateTime></DateTime></Header><UserAction></UserAction></Envelope>");
                            XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                            _msg.Text = xmlDocument.InnerXml;
                            _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                            break;
                        case "MAIN_HEADER_XGUARD_OKOGATE":
                            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><MainHeader><Number></Number><Version></Version><SystemID></SystemID></MainHeader></Envelope>");
                            XML.SetValue(xmlDocument, "/Envelope/MainHeader/Number", _msg.Get("GID").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/MainHeader/Version", _msg.Get("Version").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/MainHeader/SystemID", _msg.Get("SystemID").ToString());
                            _msg.Text = xmlDocument.InnerXml;
                            _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                            result = true;
                            return result;
                        case "CONTACTID_MSG_OKOGATE":
                            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Envelope><Header><Number></Number><Type>2</Type><DateTime></DateTime></Header><Event></Event></Envelope>");
                            XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Header/DateTime", DateTime.UtcNow.ToString("s"));
                            XML.SetValue(xmlDocument, "/Envelope/Event/ObjectNumber", _msg.Get("Object").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Event/DateTime", _msg.TimeStamp.ToUniversalTime().ToString("s"));
                            XML.SetValue(xmlDocument, "/Envelope/Event/Code", _msg.Get("EventCode").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Event/PartNumber", _msg.Get("Part").ToString());
                            XML.SetValue(xmlDocument, "/Envelope/Event/ZoneUserNumber", _msg.Get("Zone").ToString());
                            _msg.Text = xmlDocument.InnerXml;
                            _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                            result = true;
                            return result;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                }
                result = false;
                return result;
            }
            public static bool PackNext(Message _msg)
            {
                bool result;
                if (_msg.Get("PackNext").ToString() == "1")
                {
                    try
                    {
                        string type = _msg.Type;
                        if (type != null)
                        {
                            if (type == "MESSAGE_AK_OKOGATE" || type == "MESSAGE_PULT_OKOGATE")
                            {
                                int codeIndex = OKO2.GetCodeIndex(byte.Parse(_msg.Get("Class").ToString()), byte.Parse(_msg.Get("Code").ToString()));
                                if (codeIndex >= 0)
                                {
                                    OKO2.MT type2 = OKO2.OKO2MsgTable[codeIndex].Type;
                                    switch (type2)
                                    {
                                        case OKO2.MT.MT23:
                                        case OKO2.MT.MT24:
                                            {
                                                int num = (OKO2.OKO2MsgTable[codeIndex].Type - OKO2.MT.MT23) * 8 + 1;
                                                byte b = byte.Parse(_msg.Get("Part").ToString());
                                                int num2 = (int)_msg.Get("PackNextParam0");
                                                for (int i = num2 + 1; i < 8; i++)
                                                {
                                                    if ((b >> i & 1) != 0)
                                                    {
                                                        _msg.Set("PackNextParam0", i);
                                                        byte b2 = (byte)(i + num);
                                                        XmlDocument xmlDocument = new XmlDocument();
                                                        xmlDocument.LoadXml(_msg.Text);
                                                        XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                                                        XML.SetValue(xmlDocument, "/Envelope/Event/PartNumber", b2.ToString());
                                                        _msg.Text = xmlDocument.InnerXml;
                                                        _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                                                        result = true;
                                                        return result;
                                                    }
                                                }
                                                break;
                                            }
                                        default:
                                            switch (type2)
                                            {
                                                case OKO2.MT.MT53:
                                                case OKO2.MT.MT54:
                                                case OKO2.MT.MT55:
                                                case OKO2.MT.MT56:
                                                    {
                                                        int num = (OKO2.OKO2MsgTable[codeIndex].Type - OKO2.MT.MT53) * 8 + 1;
                                                        byte b = byte.Parse(_msg.Get("Part").ToString());
                                                        int num2 = (int)_msg.Get("PackNextParam0");
                                                        for (int i = num2 + 1; i < 8; i++)
                                                        {
                                                            if ((b >> i & 1) != 0)
                                                            {
                                                                _msg.Set("PackNextParam0", i);
                                                                byte b2 = (byte)(i + num);
                                                                XmlDocument xmlDocument = new XmlDocument();
                                                                xmlDocument.LoadXml(_msg.Text);
                                                                XML.SetValue(xmlDocument, "/Envelope/Header/Number", _msg.Get("GID").ToString());
                                                                XML.SetValue(xmlDocument, "/Envelope/Event/PartNumber", b2.ToString());
                                                                _msg.Text = xmlDocument.InnerXml;
                                                                _msg.Content = Encoding.UTF8.GetBytes(_msg.Text);
                                                                result = true;
                                                                return result;
                                                            }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    }
                    _msg.Set("PackNext", 0);
                }
                result = false;
                return result;
            }
            public static bool Unpack(Message _msg)
            {
                bool result;
                try
                {
                    _msg.Text = Encoding.UTF8.GetString(_msg.Content);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(_msg.Text);
                    if (xmlDocument.SelectSingleNode("/Envelope/Header") != null)
                    {
                        _msg.Set("GID", XML.GetValue(xmlDocument, "/Envelope/Header/Number"));
                        switch (SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Header/Type")))
                        {
                            case 1:
                                {
                                    _msg.Type = "ACK_CONNECT_OKOGATE";
                                    _msg.Set("ResponseGID", XML.GetValue(xmlDocument, "/Envelope/Response/Number"));
                                    string value = XML.GetValue(xmlDocument, "/Envelope/Response/Result");
                                    if (value != "")
                                    {
                                        _msg.Set("Result", value);
                                    }
                                    value = XML.GetValue(xmlDocument, "/Envelope/Response/Comment");
                                    if (value != "")
                                    {
                                        _msg.Set("Comment", value);
                                    }
                                    result = true;
                                    return result;
                                }
                            case 2:
                                {
                                    _msg.Type = "MESSAGE_PULT_OKOGATE";
                                    _msg.Set("Object", (ushort)SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Event/ObjectNumber")));
                                    _msg.Set("AlarmGroupId", SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Event/AlarmGroupID")));
                                    _msg.TimeStamp = DateTime.Parse(XML.GetValue(xmlDocument, "/Envelope/Event/DateTime")).ToLocalTime();
                                    int codeIndex = ContactID.GetCodeIndex(SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Event/Code")));
                                    if (codeIndex >= 0)
                                    {
                                        _msg.Set("Number", 1);
                                        _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                        _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                        int num = SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Event/PartNumber"));
                                        int num2 = SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Event/ZoneUserNumber"));
                                        _msg.Set("RadioRetr", 0);
                                        _msg.Set("AttrRetr", 0);
                                        switch (ContactID.CIDMsgTable[codeIndex].Type)
                                        {
                                            case OKO2.MT.MT00:
                                                _msg.Set("UnpackError", 200);
                                                result = true;
                                                return result;
                                            case OKO2.MT.MT10:
                                            case OKO2.MT.MT11:
                                            case OKO2.MT.MT12:
                                                switch (ContactID.CIDMsgTable[codeIndex].Type)
                                                {
                                                    case OKO2.MT.MT10:
                                                        if (num2 == 601)
                                                        {
                                                            num2 = 0;
                                                            num = 0;
                                                        }
                                                        else
                                                        {
                                                            if (num2 >= 600)
                                                            {
                                                                num2 -= 400;
                                                            }
                                                            if (num2 > 255)
                                                            {
                                                                num2 = 0;
                                                            }
                                                        }
                                                        break;
                                                    case OKO2.MT.MT11:
                                                        num2 = 0;
                                                        break;
                                                    case OKO2.MT.MT12:
                                                        num = 0;
                                                        num2 = 0;
                                                        break;
                                                }
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", (byte)num);
                                                _msg.Set("Zone", (byte)num2);
                                                break;
                                            case OKO2.MT.MT20:
                                            case OKO2.MT.MT21:
                                            case OKO2.MT.MT23:
                                            case OKO2.MT.MT24:
                                                switch (ContactID.CIDMsgTable[codeIndex].Type)
                                                {
                                                    case OKO2.MT.MT20:
                                                        if (num2 == 601)
                                                        {
                                                            num2 = 0;
                                                            num = 0;
                                                        }
                                                        else
                                                        {
                                                            if (num2 >= 600)
                                                            {
                                                                num2 -= 400;
                                                            }
                                                            if (num2 > 255)
                                                            {
                                                                num2 = 0;
                                                            }
                                                        }
                                                        break;
                                                    case OKO2.MT.MT21:
                                                        num2 = 0;
                                                        break;
                                                }
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", (byte)num);
                                                _msg.Set("Zone", (byte)num2);
                                                break;
                                            case OKO2.MT.MT30:
                                                if (num2 == 601)
                                                {
                                                    num2 = 0;
                                                    num = 0;
                                                }
                                                else
                                                {
                                                    if (num2 >= 600)
                                                    {
                                                        num2 -= 400;
                                                    }
                                                    if (num2 > 255)
                                                    {
                                                        num2 = 0;
                                                    }
                                                }
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", (byte)num);
                                                _msg.Set("Zone", (byte)num2);
                                                break;
                                            case OKO2.MT.MT40:
                                            case OKO2.MT.MT41:
                                            case OKO2.MT.MT42:
                                                switch (ContactID.CIDMsgTable[codeIndex].Type)
                                                {
                                                    case OKO2.MT.MT40:
                                                        if (num2 == 601)
                                                        {
                                                            num2 = 0;
                                                            num = 0;
                                                        }
                                                        else
                                                        {
                                                            if (num2 >= 600)
                                                            {
                                                                num2 -= 400;
                                                            }
                                                            if (num2 > 255)
                                                            {
                                                                num2 = 0;
                                                            }
                                                        }
                                                        break;
                                                    case OKO2.MT.MT41:
                                                        num2 = 0;
                                                        break;
                                                    case OKO2.MT.MT42:
                                                        num = 0;
                                                        num2 = 0;
                                                        break;
                                                }
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", 0);
                                                _msg.Set("Part", (byte)num);
                                                _msg.Set("Zone", (byte)num2);
                                                break;
                                            case OKO2.MT.MT50:
                                            case OKO2.MT.MT51:
                                            case OKO2.MT.MT53:
                                            case OKO2.MT.MT54:
                                            case OKO2.MT.MT55:
                                            case OKO2.MT.MT56:
                                                switch (ContactID.CIDMsgTable[codeIndex].Type)
                                                {
                                                    case OKO2.MT.MT50:
                                                        if (num2 > 900)
                                                        {
                                                            num2 = 0;
                                                        }
                                                        break;
                                                    case OKO2.MT.MT51:
                                                        num2 = 0;
                                                        break;
                                                }
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", (byte)(num2 >> 8 & 255));
                                                _msg.Set("Part", (byte)num);
                                                _msg.Set("Zone", (byte)(num2 & 255));
                                                break;
                                            case OKO2.MT.MT60:
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", 33);
                                                _msg.Set("Zone", 2);
                                                break;
                                            case OKO2.MT.MT61:
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", 33);
                                                _msg.Set("Zone", 5);
                                                break;
                                            case OKO2.MT.MT62:
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", 34);
                                                _msg.Set("Zone", 2);
                                                break;
                                            case OKO2.MT.MT63:
                                                _msg.Set("Class", ContactID.CIDMsgTable[codeIndex].Class);
                                                _msg.Set("Code", ContactID.CIDMsgTable[codeIndex].Code);
                                                _msg.Set("Part", 34);
                                                _msg.Set("Zone", 5);
                                                break;
                                            case OKO2.MT.NTF00:
                                                result = false;
                                                return result;
                                        }
                                    }
                                    else
                                    {
                                        _msg.Set("UnpackError", 201);
                                    }
                                    result = true;
                                    return result;
                                }
                            case 3:
                                {
                                    _msg.Type = "ADD_OBJECT_XGUARD_OKOGATE";
                                    _msg.Set("Object", (ushort)SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Object/Number")));
                                    _msg.Set("TypeName", XML.GetValue(xmlDocument, "/Envelope/Object/TypeName"));
                                    _msg.Set("Name", XML.GetValue(xmlDocument, "/Envelope/Object/Name"));
                                    _msg.Set("Contract", XML.GetValue(xmlDocument, "/Envelope/Object/Contract"));
                                    _msg.Set("Address", XML.GetValue(xmlDocument, "/Envelope/Object/Address"));
                                    _msg.Set("MakeDateTime", DateTime.Parse(XML.GetValue(xmlDocument, "/Envelope/Object/MakeDateTime")).ToLocalTime().ToString());
                                    _msg.Set("ActivateDateTime", DateTime.Parse(XML.GetValue(xmlDocument, "/Envelope/Object/ActivateDateTime")).ToLocalTime().ToString());
                                    _msg.Set("CommentForOperator", XML.GetValue(xmlDocument, "/Envelope/Object/CommentForOperator"));
                                    _msg.Set("CommentForGuard", XML.GetValue(xmlDocument, "/Envelope/Object/CommentForGuard"));
                                    _msg.Set("CustomersComment", XML.GetValue(xmlDocument, "/Envelope/Object/CustomersComment"));
                                    string text = "";
                                    for (int i = 0; i < 255; i++)
                                    {
                                        string value2 = XML.GetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/ID");
                                        if (value2 == "")
                                        {
                                            break;
                                        }
                                        text = text + ((i != 0) ? ";" : "") + XML.GetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/Number");
                                        text = text + ";" + XML.GetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/Name").Replace(";", ",");
                                        text = text + ";" + XML.GetValue(xmlDocument, "/Envelope/Object/Parts/Part", i, "/DeviceTypeName").Replace(";", ",");
                                    }
                                    _msg.Set("Parts", text);
                                    string text2 = "";
                                    for (int i = 0; i < 255; i++)
                                    {
                                        string value2 = XML.GetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/ID");
                                        if (value2 == "")
                                        {
                                            break;
                                        }
                                        text2 = text2 + ((i != 0) ? ";" : "") + XML.GetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/Number");
                                        text2 = text2 + ";" + XML.GetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/PartNumber");
                                        text2 = text2 + ";" + XML.GetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/Name").Replace(";", ",");
                                        text2 = text2 + ";" + XML.GetValue(xmlDocument, "/Envelope/Object/Zones/Zone", i, "/DeviceTypeName").Replace(";", ",");
                                    }
                                    _msg.Set("Zones", text2);
                                    result = true;
                                    return result;
                                }
                            case 4:
                                {
                                    _msg.Type = "DEL_OBJECT_XGUARD_OKOGATE";
                                    string value3 = XML.GetValue(xmlDocument, "/Envelope/Object/ID");
                                    _msg.Set("Object", (ushort)SStr.StrToInt(XML.GetValue(xmlDocument, "/Envelope/Object/Number")));
                                    result = true;
                                    return result;
                                }
                            case 5:
                                _msg.Type = "USER_ACTION_XGUARD_OKOGATE";
                                break;
                            case 6:
                                _msg.Type = "TEST_CONNECT_OKOGATE";
                                result = true;
                                return result;
                        }
                    }
                    else
                    {
                        if (xmlDocument.SelectSingleNode("/Envelope/MainHeader") != null)
                        {
                            _msg.Type = "MAIN_HEADER_XGUARD_OKOGATE";
                            _msg.Set("GID", XML.GetValue(xmlDocument, "/Envelope/MainHeader/Number"));
                            _msg.Set("Version", XML.GetValue(xmlDocument, "/Envelope/MainHeader/Version"));
                            _msg.Set("SystemID", XML.GetValue(xmlDocument, "/Envelope/MainHeader/SystemID"));
                            result = true;
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                }
                result = false;
                return result;
            }
            public static int GetPacketSize(byte[] buffer, int size)
            {
                int result;
                for (int i = 0; i < XMLReader.BytePacketsEnds.Length; i++)
                {
                    int num = SStr.IndexOf(buffer, 0, size, XMLReader.BytePacketsEnds[i]);
                    if (num >= 0)
                    {
                        result = num + XMLReader.BytePacketsEnds[i].Length;
                        return result;
                    }
                }
                result = 0;
                return result;
            }
        }
        public const string Version = "1";
        public static string[] PacketsEnds = new string[]
        {
            "</Envelope>"
        };
        public static byte[][] BytePacketsEnds = null;
        public static XMDBConnect mdbconnect = null;
    }
}