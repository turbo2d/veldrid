﻿using Vulkan;
using static Vulkan.VulkanNative;
using static Veldrid.Vk.VulkanUtil;
using System;

namespace Veldrid.Vk
{
    internal unsafe class VkShader : Shader
    {
        private readonly VkGraphicsDevice _gd;
        private readonly VkShaderModule _shaderModule;
        private bool _disposed;

        public VkShaderModule ShaderModule => _shaderModule;

        public VkShader(VkGraphicsDevice gd, ref ShaderDescription description)
        {
            _gd = gd;

            VkShaderModuleCreateInfo shaderModuleCI = VkShaderModuleCreateInfo.New();
            fixed (byte* codePtr = description.ShaderBytes)
            {
                shaderModuleCI.codeSize = (UIntPtr)description.ShaderBytes.Length;
                shaderModuleCI.pCode = (uint*)codePtr;
                VkResult result = vkCreateShaderModule(gd.Device, ref shaderModuleCI, null, out _shaderModule);
                CheckResult(result);
            }
        }

        public override void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                vkDestroyShaderModule(_gd.Device, ShaderModule, null);
            }
        }
    }
}
