global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Threading;
global using System.Threading.Channels;
global using System.Threading.Tasks;

global using System.Globalization;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Documents;
global using System.Windows.Data;
global using System.Windows.Media;
global using System.Windows.Input;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using MediatR;
global using Mapster;

global using Ops.Exchange;
global using Ops.Exchange.DependencyInjection;
global using Ops.Exchange.Forwarder;
global using Ops.Exchange.Management;
global using Ops.Exchange.Model;

global using Ops.Host.Common;
global using Ops.Host.Common.Extensions;
global using Ops.Host.Common.Utils;

global using Ops.Host.Core;
global using Ops.Host.Core.Dtos;
global using Ops.Host.Core.Extensions;
global using Ops.Host.Core.Entity;
global using Ops.Host.Core.Management;
global using Ops.Host.Core.Services;

global using Ops.Host.Shared.Component;
global using Ops.Host.Shared.Options;
global using Ops.Host.Shared.ViewModel;

global using Ops.Host.App.Models;
