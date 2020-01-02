using System;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCube
{
	float f000, f001, f010, f011, f100, f101, f110, f111;
	float x0, y0, z0, x1, y1, z1;
	List<Vector3> vertices;

	
	public MarchingCube()
	{

	}

	public void SetF(float f0, float f1, float f2, float f3, float f4, float f5, float f6, float f7)
	{
		f000 = f0;
		f001 = f1;
		f010 = f2;
		f011 = f3;
		f100 = f4;
		f101 = f5;
		f110 = f6;
		f111 = f7;
	}

	public void SetXYZ(float _x0, float _y0, float _z0, float _x1, float _y1, float _z1)
	{
		x0 = _x0;
		y0 = _y0;
		z0 = _z0;
		x1 = _x1;
		y1 = _y1;
		z1 = _z1;
	}

	public void SetList(List<Vector3> _vertices)
	{
		vertices = _vertices;
	}

    float InnerDivision(float x1, float x2, float rate1, float rate2)
    {
        float r1 = Mathf.Abs(rate1);
        float r2 = Mathf.Abs(rate2);
        return (r2 * x1 + r1 * x2) / (r1 + r2);
    }
    public void MarchingCube1()
    {
        int verticesCount = vertices.Count;
        float x00 = InnerDivision(x0, x1, f000, f100);
        float x01 = InnerDivision(x0, x1, f001, f101);
        float x10 = InnerDivision(x0, x1, f010, f110);
        float x11 = InnerDivision(x0, x1, f011, f111);
        float y00 = InnerDivision(y0, y1, f000, f010);
        float y01 = InnerDivision(y0, y1, f001, f011);
        float y10 = InnerDivision(y0, y1, f100, f110);
        float y11 = InnerDivision(y0, y1, f101, f111);
        float z00 = InnerDivision(z0, z1, f000, f001);
        float z01 = InnerDivision(z0, z1, f010, f011);
        float z10 = InnerDivision(z0, z1, f100, f101);
        float z11 = InnerDivision(z0, z1, f110, f111);

        int b0 = 0, b1 = 0, b2 = 0, b3 = 0, b4 = 0, b5 = 0, b6 = 0, b7 = 0;
        if (f000 > 0) b0 = 1;
        if (f001 > 0) b1 = 1;
        if (f010 > 0) b2 = 1;
        if (f011 > 0) b3 = 1;
        if (f100 > 0) b4 = 1;
        if (f101 > 0) b5 = 1;
        if (f110 > 0) b6 = 1;
        if (f111 > 0) b7 = 1;
        int code = b0 * 128 + b1 * 64 + b2 * 32 + b3 * 16 + b4 * 8 + b5 * 4 + b6 * 2 + b7;
        int jenre = b0 + b1 + b2 + b3 + b4 + b5 + b6 + b7;

        switch (code)
        {
            case 0b10000000: // f000
            case 0b01111111: // f000
                VerticesAdd(0, 0, 0, 1, 0, 0);
                VerticesAdd(0, 0, 0, 0, 1, 0);
                VerticesAdd(0, 0, 0, 0, 0, 1);
                break;
            case 0b01000000: // f001 
            case 0b10111111: // f001  
                VerticesAdd(0, 0, 1, 1, 0, 1); ;// f001 - f101
                VerticesAdd(0, 0, 1, 0, 1, 1); ;// f001 - f101
                VerticesAdd(0, 0, 1, 0, 0, 0); ;// f001 - f101
                break;
            case 0b00100000: // f010 
            case 0b11011111: // f010 
                VerticesAdd(0, 1, 0, 1, 1, 0);// f110
                VerticesAdd(0, 1, 0, 0, 0, 0);// f000
                VerticesAdd(0, 1, 0, 0, 1, 1);// f011
                break;
            case 0b00010000: // f011 
            case 0b11101111: // f011 
                VerticesAdd(0, 1, 1, 1, 1, 1); // f111
                VerticesAdd(0, 1, 1, 0, 0, 1);// f001
                VerticesAdd(0, 1, 1, 0, 1, 0);// f010
                break;
            case 0b00001000: // f100 
            case 0b11110111: // f100 
                VerticesAdd(1, 0, 0, 0, 0, 0);// f000
                VerticesAdd(1, 0, 0, 1, 1, 0);// f110
                VerticesAdd(1, 0, 0, 1, 0, 1);// f101
                break;
            case 0b00000100: // f101 
            case 0b11111011: // f101 
                VerticesAdd(1, 0, 1, 0, 0, 1);// f001
                VerticesAdd(1, 0, 1, 1, 1, 1);// f111
                VerticesAdd(1, 0, 1, 1, 0, 0);// f100
                break;
            case 0b00000010: // f110 
            case 0b11111101: // f110 
                VerticesAdd(1, 1, 0, 0, 1, 0);// f010
                VerticesAdd(1, 1, 0, 1, 0, 0);// f100
                VerticesAdd(1, 1, 0, 1, 1, 1);// f111
                break;
            case 0b00000001: // f111 
            case 0b11111110: // f111 
                VerticesAdd(1, 1, 1, 0, 1, 1);// f011
                VerticesAdd(1, 1, 1, 1, 0, 1);// f101
                VerticesAdd(1, 1, 1, 1, 1, 0);// f110
                break;
            //////////
            //４角形ここから
            //////////
            case 0b10001000: // f000 f100
            case 0b01110111: // f000 f100
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                break;
            case 0b00100010: // f010 f110
            case 0b11011101: // f010 f110
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                break;
            case 0b01000100: // f001 f101 
            case 0b10111011: // f001 f101
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                break;
            case 0b00010001: // f011 f111 
            case 0b11101110: // f011 f111
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001 
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                break;
            case 0b10100000: // f000 f010
            case 0b01011111: // f000 f010
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                break;
            case 0b01010000: // f001 f011
            case 0b10101111: // f001 f011
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                break;
            case 0b00001010: // f100 f110
            case 0b11110101: // f100 f110
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                break;
            case 0b00000101: // f101 f111
            case 0b11111010: // f101 f111
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                break;
            case 0b11000000: // f000 f001
            case 0b00111111: // f000 f001
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                break;
            case 0b00110000: // f010 f011
            case 0b11001111: // f010 f011
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                break;
            case 0b00001100: // f100 f101
            case 0b11110011: // f100 f101
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                break;
            case 0b00000011: // f110 f111
            case 0b11111100: // f110 f111
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                break;
            //////////
            //4角形パート２
            //////////
            case 0b10101010: // 000-100-110-010
            case 0b01010101:
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                break;
            case 0b11001100: // 000-001-101-100
            case 0b00110011:
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                break;
            case 0b11110000: // 000-010-011-001
            case 0b00001111:
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                break;
            //////////
            //4角形パート３
            //////////
            case 0b10000100: //000-101
            case 0b01111011:
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                break;
            case 0b00010010: //011-110
            case 0b11101101:
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                break;
            //////////
            //5角形ここから
            //////////
            case 0b10101000: // 100-000-010
            case 0b01010111: // 
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                break;
            case 0b10100010: // 000-010-110
            case 0b01011101: // 
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                break;
            case 0b10001010: // 000-100-110
            case 0b01110101: // 
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                break;
            case 0b00101010: // 100-110-010
            case 0b11010101: // 
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                break;
            ///////
            case 0b01010100: // 101-001-011
            case 0b10101011: // 
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                break;
            case 0b01010001: // 001-011-111
            case 0b10101110: // 
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                break;
            case 0b01000101: // 001-101-111
            case 0b10111010: // 
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                break;
            case 0b00010101: // 101-111-011
            case 0b11101010: // 
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                break;
            /////////
            case 0b11001000: // 001-000-100
            case 0b00110111: // 
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                break;
            case 0b10001100: // 000-100-101
            case 0b01110011: // 
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                break;
            case 0b11000100: // 000-001-101
            case 0b00111011: // 
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                break;
            case 0b01001100: // 001-101-100
            case 0b10110011: // 
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                break;
            ////
            case 0b00110010: // 011-010-110
            case 0b11001101: // 
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                break;
            case 0b00100011: // 010-110-111
            case 0b11011100: // 
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                break;
            case 0b00110001: // 010-011-111
            case 0b11001110: // 
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                break;
            case 0b00010011: // 011-111-110
            case 0b11101100: // 
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                break;
            ////
            case 0b11100000: // 010-000-001
            case 0b00011111: // 
                VerticesAdd(0, 1, 0, 0, 1, 1);// f010 - f011
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                break;
            case 0b11010000: // 011-001-000
            case 0b00101111: // 
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                break;
            case 0b10110000: // 000-010-011
            case 0b01001111: // 
                VerticesAdd(0, 0, 0, 0, 0, 1);// f000 - f001
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                break;
            case 0b01110000: // 001-011-010
            case 0b10001111: // 
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 0, 1, 1, 0, 1);// f001 - f101
                VerticesAdd(0, 1, 1, 1, 1, 1);// f011 - f111
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                break;
            ////
            case 0b00001110: // 110-100-101
            case 0b11110001: // 
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                break;
            case 0b00001101: // 111-101-100
            case 0b11110010: // 
                VerticesAdd(1, 1, 1, 1, 1, 0);// f111 - f110
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                break;
            case 0b00001011: // 100-110-111
            case 0b11110100: // 
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 0, 0, 0, 0, 0);// f100 - f000
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                break;
            case 0b00000111: // 101-111-110
            case 0b11111000: // 
                VerticesAdd(1, 0, 1, 1, 0, 0);// f101 - f100
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                VerticesAdd(1, 1, 0, 0, 1, 0);// f110 - f010
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                break;
            //////////
            //5角形パート２ある？
            //////////
            //////////
            //６角形ここから
            //////////
            case 0b11101000: // 000-(100,010,001)
            case 0b00010111:
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(0, 0, 1, 0, 1, 1);// f001 - f011
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(1, 1, 0, 1, 0, 0);// f110 - f100
                break;
            case 0b11010100: // 001-(101,011,000)
            case 0b00101011:
                VerticesAdd(1, 0, 1, 1, 1, 1);// f101 - f111
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                VerticesAdd(0, 1, 1, 0, 1, 0);// f011 - f010
                VerticesAdd(0, 1, 0, 0, 0, 0);// f010 - f000
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(1, 0, 0, 1, 0, 1);// f100 - f101
                break;
            case 0b10110010: // 010-(110,000,011)
            case 0b01001101:
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                VerticesAdd(1, 1, 1, 0, 1, 1);// f111 - f011
                VerticesAdd(0, 1, 1, 0, 0, 1);// f011 - f001
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 0, 0, 1, 0, 0);// f000 - f100
                VerticesAdd(1, 0, 0, 1, 1, 0);// f100 - f110
                break;
            case 0b01110001: // 011-(111,001,010)
            case 0b10001110:
                VerticesAdd(1, 1, 1, 1, 0, 1);// f111 - f101
                VerticesAdd(1, 0, 1, 0, 0, 1);// f101 - f001
                VerticesAdd(0, 0, 1, 0, 0, 0);// f001 - f000
                VerticesAdd(0, 0, 0, 0, 1, 0);// f000 - f010
                VerticesAdd(0, 1, 0, 1, 1, 0);// f010 - f110
                VerticesAdd(1, 1, 0, 1, 1, 1);// f110 - f111
                break;
            ////////
            //６角形その２
            ////////
            case 0b10001101: // 000-100-101-111
            case 0b01110010:
                VerticesAdd(0, 0, 0, 0, 0, 1);// 000 001
                VerticesAdd(0, 0, 0, 0, 1, 0);// 000 010
                VerticesAdd(1, 0, 0, 1, 1, 0);// 100 110
                VerticesAdd(1, 0, 1, 1, 1, 0);// 111 110
                VerticesAdd(1, 1, 1, 0, 1, 1);// 111 011
                VerticesAdd(1, 1, 1, 0, 0, 1);// 101 001
                break;
            case 0b01010011: // 001-011-111-110
            case 0b10101100:
                VerticesAdd(0, 0, 1, 1, 0, 1);// 001 101
                VerticesAdd(0, 0, 1, 0, 0, 0);// 001 000
                VerticesAdd(0, 1, 1, 0, 1, 0);// 011 010
                VerticesAdd(1, 1, 0, 0, 1, 0);// 110 010
                VerticesAdd(1, 1, 0, 1, 0, 0);// 110 100
                VerticesAdd(1, 1, 1, 1, 0, 1);// 111 101
                break;
            case 0b11001010: // 001-000-100-110
            case 0b00110101:
                VerticesAdd(0, 0, 1, 1, 0, 1);// 001 101
                VerticesAdd(0, 0, 1, 0, 1, 1);// 001 011
                VerticesAdd(0, 0, 0, 0, 1, 0);// 000 010
                VerticesAdd(1, 1, 0, 0, 1, 0);// 110 010
                VerticesAdd(1, 1, 0, 1, 1, 1);// 110 111
                VerticesAdd(1, 0, 0, 1, 0, 1









                    );// 100 101
                break;
            default:
                if (code != 0 && code != 255)
                    Debug.Log(code);
                break;
        }
    }

    void VerticesAdd(int fx, int fy, int fz, int gx, int gy, int gz)
    {
        int code = 32 * fx + 16 * fy + 8 * fz + 4 * gx + 2 * gy + gz;
        switch (code)
        {
            case 0b000100:
            case 0b100000:
                float x00 = InnerDivision(x0, x1, f000, f100);
                vertices.Add(new Vector3(x00, y0, z0));// f000 - f100
                break;
            case 0b001101:
            case 0b101001:
                float x01 = InnerDivision(x0, x1, f001, f101);
                vertices.Add(new Vector3(x01, y0, z1));// f001 - f101
                break;
            case 0b010110:
            case 0b110010:
                float x10 = InnerDivision(x0, x1, f010, f110);
                vertices.Add(new Vector3(x10, y1, z0));// f010 - f110
                break;
            case 0b011111:
            case 0b111011:
                float x11 = InnerDivision(x0, x1, f011, f111);
                vertices.Add(new Vector3(x11, y1, z1));// f011 - f111
                break;
            case 0b000010:
            case 0b010000:
                float y00 = InnerDivision(y0, y1, f000, f010);
                vertices.Add(new Vector3(x0, y00, z0));// f000 - f010
                break;
            case 0b001011:
            case 0b011001:
                float y01 = InnerDivision(y0, y1, f001, f011);
                vertices.Add(new Vector3(x0, y01, z1));// f001 - f011
                break;
            case 0b100110:
            case 0b110100:
                float y10 = InnerDivision(y0, y1, f100, f110);
                vertices.Add(new Vector3(x1, y10, z0));// f100 - f110
                break;
            case 0b101111:
            case 0b111101:
                float y11 = InnerDivision(y0, y1, f101, f111);
                vertices.Add(new Vector3(x1, y11, z1));// f101 - f111
                break;
            case 0b000001:
            case 0b001000:
                float z00 = InnerDivision(z0, z1, f000, f001);
                vertices.Add(new Vector3(x0, y0, z00));// f000 - f001
                break;
            case 0b010011:
            case 0b011010:
                float z01 = InnerDivision(z0, z1, f010, f011);
                vertices.Add(new Vector3(x0, y1, z01));// f010 - f011
                break;
            case 0b100101:
            case 0b101100:
                float z10 = InnerDivision(z0, z1, f100, f101);
                vertices.Add(new Vector3(x1, y0, z10));// f100 - f101
                break;
            case 0b110111:
            case 0b111110:
                float z11 = InnerDivision(z0, z1, f110, f111);
                vertices.Add(new Vector3(x1, y1, z11));// f110 - f111
                break;

        }
    }

}
