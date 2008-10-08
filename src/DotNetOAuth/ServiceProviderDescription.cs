﻿//-----------------------------------------------------------------------
// <copyright file="ServiceProviderDescription.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOAuth {
	using System;
	using System.Diagnostics;
	using System.Linq;
	using DotNetOAuth.ChannelElements;
	using DotNetOAuth.Messaging;

	/// <summary>
	/// A description of the endpoints on a Service Provider.
	/// </summary>
	public class ServiceProviderDescription {
		/// <summary>
		/// The field used to store the value of the <see cref="RequestTokenEndpoint"/> property.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MessageReceivingEndpoint requestTokenEndpoint;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceProviderDescription"/> class.
		/// </summary>
		public ServiceProviderDescription() {
		}

		/// <summary>
		/// Gets or sets the URL used to obtain an unauthorized Request Token,
		/// described in Section 6.1 (Obtaining an Unauthorized Request Token).
		/// </summary>
		/// <remarks>
		/// The request URL query MUST NOT contain any OAuth Protocol Parameters.
		/// This is the URL that <see cref="Messages.RequestTokenMessage"/> messages are directed to.
		/// </remarks>
		/// <exception cref="ArgumentException">Thrown if this property is set to a URI with OAuth protocol parameters.</exception>
		public MessageReceivingEndpoint RequestTokenEndpoint {
			get {
				return this.requestTokenEndpoint;
			}

			set {
				if (value != null && UriUtil.QueryStringContainsOAuthParameters(value.Location)) {
					throw new ArgumentException(Strings.RequestUrlMustNotHaveOAuthParameters);
				}

				this.requestTokenEndpoint = value;
			}
		}

		/// <summary>
		/// Gets or sets the URL used to obtain User authorization for Consumer access, 
		/// described in Section 6.2 (Obtaining User Authorization).
		/// </summary>
		/// <remarks>
		/// This is the URL that <see cref="Messages.DirectUserToServiceProviderMessage"/> messages are
		/// indirectly (via the user agent) sent to.
		/// </remarks>
		public MessageReceivingEndpoint UserAuthorizationEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the URL used to exchange the User-authorized Request Token 
		/// for an Access Token, described in Section 6.3 (Obtaining an Access Token).
		/// </summary>
		/// <remarks>
		/// This is the URL that <see cref="Messages.RequestAccessTokenMessage"/> messages are directed to.
		/// </remarks>
		public MessageReceivingEndpoint AccessTokenEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the signing policies that apply to this Service Provider.
		/// </summary>
		public ITamperProtectionChannelBindingElement[] TamperProtectionElements { get; set; }

		/// <summary>
		/// Creates a signing element that includes all the signing elements this service provider supports.
		/// </summary>
		/// <returns>The created signing element.</returns>
		internal ITamperProtectionChannelBindingElement CreateTamperProtectionElement() {
			return new SigningBindingElementChain(this.TamperProtectionElements.Select(el => (ITamperProtectionChannelBindingElement)el.Clone()).ToArray());
		}
	}
}
