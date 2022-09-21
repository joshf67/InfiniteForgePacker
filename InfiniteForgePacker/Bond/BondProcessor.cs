// <copyright file="BondProcessor.cs" company="Den Delimarsky">
// Developed by Den Delimarsky.
// Den Delimarsky licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>

using System;
using System.IO;
using System.Text;
using Bond;
using Bond.IO.Unsafe;
using Bond.Protocols;
using Schemas;

namespace BondReader
{
    /// <summary>
    /// Class responsible for processing Bond files.
    /// </summary>
    internal class BondProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BondProcessor"/> class, providing the ability to process a Bond file.
        /// </summary>
        /// <param name="version">Bond protocol version.</param>
        internal BondProcessor(ushort version)
        {
            this.Version = version;
            this.LogContainer = new StringBuilder();
        }

        private ushort Version { get; set; }

        private StringBuilder LogContainer { get; set; }

        public static BContainer MainContainer = new BContainer(BondDataType.BT_STRUCT);

        /// <summary>
        /// Processes the Bond file and outputs data to an external file, if applicable.
        /// </summary>
        /// <param name="inputFilePath">Path to the input file to be processed.</param>
        /// <param name="outputFilePath">Path to output file to be processed.</param>
        internal void ProcessFile(string inputFilePath, string outputFilePath)
        {
            var inputBuffer = new Bond.IO.Unsafe.InputBuffer(File.ReadAllBytes(inputFilePath));
            var reader = new CompactBinaryReader<Bond.IO.Unsafe.InputBuffer>(inputBuffer, this.Version);
            this.ReadData(reader, MainContainer);

            if (!string.IsNullOrWhiteSpace(outputFilePath))
            {
                try
                {
                    File.WriteAllText(outputFilePath, this.LogContainer.ToString());
                }
                catch (Exception ex)
                {
                    this.Log($"Failed to write file to {outputFilePath}.", this.LogContainer);
                    this.Log(ex.Message, this.LogContainer);
                }
            }
        }

        internal void ProcessFile(string inputFilePath)
        {
            
            var inputBuffer = new Bond.IO.Unsafe.InputBuffer(File.ReadAllBytes(inputFilePath));
            var reader = new CompactBinaryReader<Bond.IO.Unsafe.InputBuffer>(inputBuffer, this.Version);
            this.ReadData(reader, MainContainer);
             inputBuffer.Position = 0;

             var dst = Deserialize<ForgeMap>.From(reader);
            //
            // var output = new OutputStream(new FileStream(inputFilePath + "2", FileMode.Create));
            // var writer = new CompactBinaryWriter<OutputStream>(output);  
            // Serialize.To(writer, dst);
            // output.Flush();
        }

        private void ReadData(CompactBinaryReader<Bond.IO.Unsafe.InputBuffer> reader, BContainer parent)
        {
            this.Log($"█{new string('░', 25)} STR {new string('░', 25)}█", this.LogContainer);

            reader.ReadStructBegin();

            BondDataType dataType;

            do
            {
                reader.ReadFieldBegin(out dataType, out ushort fieldId);
                this.Log($"Data type: {dataType,15}\tField ID: {fieldId,15}", this.LogContainer);

                this.DecideOnDataType(reader, dataType, parent, fieldId);

                reader.ReadFieldEnd();
            } while (dataType != BondDataType.BT_STOP);

            reader.ReadStructEnd();

            this.Log($"█{new string('░', 25)} EST {new string('░', 25)}█", this.LogContainer);
        }

        private void ReadContainer(CompactBinaryReader<Bond.IO.Unsafe.InputBuffer> reader, BContainer parent, bool isMap = false)
        {
            string marker = "Mapped value type: ";
            int containerCounter;
            BondDataType containerDataType = BondDataType.BT_UNAVAILABLE;
            BondDataType valueDataType = BondDataType.BT_UNAVAILABLE;

            if (!isMap)
            {
                reader.ReadContainerBegin(out containerCounter, out containerDataType);
            }
            else
            {
                reader.ReadContainerBegin(out containerCounter, out containerDataType, out valueDataType);
            }

            this.Log($"Reading container with item type: {containerDataType,15}\tItems in container: {containerCounter,15}\t{(isMap ?  (marker + valueDataType) : string.Empty),15}", this.LogContainer);
            this.Log($"╔{new string('═', 25)} CONT {new string('═', 25)}╗", this.LogContainer);

            for (int i = 0; i < containerCounter; i++)
            {
                this.Log("Traversing list item: " + i, this.LogContainer);
                this.DecideOnDataType(reader, containerDataType, parent, -1);
                if (isMap)
                {
                    this.DecideOnDataType(reader, valueDataType, parent, -1);
                }
            }

            this.Log("Done reading container.", this.LogContainer);
            this.Log($"╚{new string('═', 25)} ECON {new string('═', 25)}╝", this.LogContainer);

            reader.ReadContainerEnd();
        }

        private void DecideOnDataType(CompactBinaryReader<Bond.IO.Unsafe.InputBuffer> reader, BondDataType dataType, BContainer parent, int id)
        {
            switch (dataType)
            {
                case BondDataType.BT_STRUCT:
                    {
                        this.ReadData(reader, parent.AddChildContainer(new BContainer(BondDataType.BT_STRUCT, id)));
                        break;
                    }

                case BondDataType.BT_LIST:
                    {
                        this.ReadContainer(reader, parent.AddChildContainer(new BContainer(BondDataType.BT_LIST, id)));
                        break;
                    }

                case BondDataType.BT_SET:
                    {
                        this.ReadContainer(reader, parent.AddChildContainer(new BContainer(BondDataType.BT_SET, id)));
                        break;
                    }

                case BondDataType.BT_MAP:
                    {
                        this.ReadContainer(reader, parent.AddChildContainer(new BContainer(BondDataType.BT_MAP, id)), true);
                        break;
                    }

                case BondDataType.BT_STRING:
                    {
                        var stringValue = reader.ReadString();
                        parent.AddChild(new BElement(BondDataType.BT_STRING, stringValue, id));
                        this.Log(stringValue, this.LogContainer);
                        break;
                    }

                case BondDataType.BT_WSTRING:
                    {
                        var stringValue = reader.ReadWString();
                        parent.AddChild(new BElement(BondDataType.BT_WSTRING, stringValue, id));
                        this.Log(stringValue, this.LogContainer);
                        break;
                    }

                case BondDataType.BT_BOOL:
                    {
                        var boolValue = reader.ReadBool();
                        parent.AddChild(new BElement(BondDataType.BT_BOOL, boolValue, id));
                        this.Log(boolValue.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_DOUBLE:
                    {
                        double doubleValue = reader.ReadDouble();
                        parent.AddChild(new BElement(BondDataType.BT_DOUBLE, doubleValue, id));
                        this.Log(doubleValue.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_FLOAT:
                    {
                        double floatValue = reader.ReadFloat();
                        parent.AddChild(new BElement(BondDataType.BT_FLOAT, floatValue, id));
                        this.Log(floatValue.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_INT8:
                    {
                        var int8value = reader.ReadInt8();
                        parent.AddChild(new BElement(BondDataType.BT_INT8, int8value, id));
                        this.Log(int8value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_INT16:
                    {
                        var int16value = reader.ReadInt16();
                        parent.AddChild(new BElement(BondDataType.BT_INT16, int16value, id));
                        this.Log(int16value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_INT32:
                    {
                        var int32Value = reader.ReadInt32();
                        parent.AddChild(new BElement(BondDataType.BT_INT32, int32Value, id));
                        this.Log(int32Value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_INT64:
                    {
                        var int64Value = reader.ReadInt64();
                        parent.AddChild(new BElement(BondDataType.BT_INT64, int64Value, id));
                        this.Log(int64Value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_UINT8:
                    {
                        var uint8value = reader.ReadUInt8();
                        parent.AddChild(new BElement(BondDataType.BT_UINT8, uint8value, id));
                        this.Log(uint8value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_UINT16:
                    {
                        var uint16Value = reader.ReadUInt16();
                        parent.AddChild(new BElement(BondDataType.BT_UINT16, uint16Value, id));
                        this.Log(uint16Value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_UINT32:
                    {
                        var uint32Value = reader.ReadUInt32();
                        parent.AddChild(new BElement(BondDataType.BT_UINT32, uint32Value, id));
                        this.Log(uint32Value.ToString(), this.LogContainer);
                        break;
                    }

                case BondDataType.BT_UINT64:
                    {
                        var uint64Value = reader.ReadUInt64();
                        parent.AddChild(new BElement(BondDataType.BT_UINT64, uint64Value, id));
                        this.Log(uint64Value.ToString(), this.LogContainer);
                        break;
                    }

                default:
                    if (dataType != BondDataType.BT_STOP && dataType != BondDataType.BT_STOP_BASE)
                    {
                        this.Log($"Skipping datatype: {dataType,10}", this.LogContainer);
                        reader.Skip(dataType);
                    }

                    break;
            }
        }

        private void Log(string value, StringBuilder logContainer)
        {
            Console.WriteLine(value);
            logContainer.AppendLine(value);
        }
    }
}
