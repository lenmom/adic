﻿using UnityEngine;
using System.Collections;

namespace Adic {
	/// <summary>
	/// Basic implementation of a command.
	/// </summary>
	public abstract class Command : ICommand {
		/// <summary>The command dispatcher that dispatched this command.</summary>
		public CommandDispatcher dispatcher { get; set; }
		/// <summary>Indicates whether the command is running.</summary>
		public bool running { get; set; }
		/// <summary>Indicates whether the command must be kept alive even after its execution.</summary>
		public bool keepAlive { get; set; }
		/// <summary>
		/// Indicates whether this command is a singleton (there's only one instance of it).
		/// 
		/// Singleton commands improve performance and are the recommended approach when, for every execution
		/// of your command, there's no need to reinject every dependency or all parameters the command needs
		/// are passed through the <code>Execute()</code> method.
		/// </summary>
		public bool singleton { get { return false; } }
		/// <summary>The quantity of the command to preload on pool.</summary>
		public int preloadPoolSize { get { return 1; } }
		/// <summary>The maximum size pool for this command.</summary>
		public int maxPoolSize { get { return 10; } }
		
		/// <summary>
		/// Executes the command.
		/// <param name="parameters">Command parameters.</param>
		public abstract void Execute(params object[] parameters);
		
		/// <summary>
		/// Retains the command as in use, not disposing it after execution.
		/// 
		/// Always call Release() after the command has terminated.
		/// </summary>
		public void Retain() {
			this.keepAlive = true;
		}
		
		/// <summary>
		/// Release this command.
		/// </summary>
		public void Release() {
			this.keepAlive = false;

			this.dispatcher.Release(this);
		}
		
		/// <summary>
		/// Dispose this command.
		/// 
		/// When overriding, always call <code>base.Dispose()</code>.
		/// </summary>
		public virtual void Dispose() {
			this.running = false;
			this.keepAlive = false;
		}
	}
}