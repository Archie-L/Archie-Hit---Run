//////////////////////////////////////////////////////
// MK Glow Camera Data HDrP	    	                //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
/// 
namespace MK.Glow.HDRP
{
    internal class CameraDataHDRP : CameraData
    {
        public static implicit operator CameraDataHDRP(UnityEngine.Rendering.HighDefinition.HDCamera input)
        {
            CameraDataHDRP data = new CameraDataHDRP();

            data.width = input.actualWidth;
            data.height = input.actualHeight;
            data.stereoEnabled = input.camera.stereoEnabled;
            data.aspect = input.camera.aspect;
            data.worldToCameraMatrix = input.camera.worldToCameraMatrix;
            data.overwriteDescriptor = true;

            return data;
        }
    }
}