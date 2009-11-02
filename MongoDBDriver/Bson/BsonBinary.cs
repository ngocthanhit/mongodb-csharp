﻿using System;
using System.Text;

namespace MongoDB.Driver.Bson
{
	public class BsonBinary:BsonType
    {
        private byte[] val;
        public byte[] Val {
            get {return val;}
            set {val = value;}
        }    

        public BsonBinary() { }

        public BsonBinary(byte[] bytes, byte subtype){
            this.val = bytes;
            this.subtype = subtype;
        }

        public BsonBinary(Binary binary){
            this.val = binary.Bytes;
            this.subtype = binary.Subtype;            
        }

        private byte subtype;
        public byte Subtype{
            get { return this.subtype; }
            set { this.subtype = value; }
        }

        public int Size{
            get {
				int size = 4; //size int
				size += 1; //subtype
				if(this.Subtype == 2){
					size += 4; //embedded size int
				}
				size += this.Val.Length;
				return size; 
			}
        }

        public byte TypeNum{
            get { return (byte)BsonDataType.Binary; }
        }


        public int Read(BsonReader reader){
            int size = reader.ReadInt32();
            int bytesRead = 4;
            this.Subtype = reader.ReadByte();
			bytesRead += sizeof(byte);
			if(this.Subtype == 2){
				size = reader.ReadInt32();
				bytesRead += 4;
			}
            this.Val = reader.ReadBytes(size);
            bytesRead += size;
            return bytesRead;
        }   

        public void Write(BsonWriter writer){
            writer.Write(this.Size);
            writer.Write(this.Subtype);
			if(this.Subtype ==2){
				writer.Write(this.Val.Length);
			}
            writer.Write(this.Val);            
        }

        public virtual object ToNative(){
            
            return new Binary(this.Val);
        }

        public override string ToString()
        {
            return string.Format("[BsonBinary: Val={0}, TypeNum={1}, Size={2}, Subtype={3}]", Val, TypeNum, Size, Subtype);
        }

	}
}
