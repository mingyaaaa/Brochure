// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/roles.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto.Roles.Grpc {

  /// <summary>Holder for reflection information generated from Protos/roles.proto</summary>
  public static partial class RolesReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/roles.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RolesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJQcm90b3Mvcm9sZXMucHJvdG8SEFByb3RvLlJvbGVzLkdycGMiIQoFUm9s",
            "ZXMSCgoCaWQYASABKAkSDAoEbmFtZRgCIAEoCSIbCgxSb2xlc1JlcXVlc3QS",
            "CwoDSWRzGAEgAygJIjoKEE11dGlSb2xlc1JlcXVlc3QSJgoFUm9sZXMYASAD",
            "KAsyFy5Qcm90by5Sb2xlcy5HcnBjLlJvbGVzIh4KD0ZhaWxJZHNSZXNwb25z",
            "ZRILCgNJZHMYASADKAkiNwoNUm9sZXNSZXNwb25zZRImCgVSb2xlcxgBIAMo",
            "CzIXLlByb3RvLlJvbGVzLkdycGMuUm9sZXMynQMKDFJvbGVzU2VydmljZRJL",
            "CghHZXRSb2xlcxIeLlByb3RvLlJvbGVzLkdycGMuUm9sZXNSZXF1ZXN0Gh8u",
            "UHJvdG8uUm9sZXMuR3JwYy5Sb2xlc1Jlc3BvbnNlElAKC0RlbGV0ZVJvbGVz",
            "Eh4uUHJvdG8uUm9sZXMuR3JwYy5Sb2xlc1JlcXVlc3QaIS5Qcm90by5Sb2xl",
            "cy5HcnBjLkZhaWxJZHNSZXNwb25zZRJQCgtVcGRhdGVSb2xlcxIeLlByb3Rv",
            "LlJvbGVzLkdycGMuUm9sZXNSZXF1ZXN0GiEuUHJvdG8uUm9sZXMuR3JwYy5G",
            "YWlsSWRzUmVzcG9uc2USSQoGSW5zZXJ0Eh4uUHJvdG8uUm9sZXMuR3JwYy5S",
            "b2xlc1JlcXVlc3QaHy5Qcm90by5Sb2xlcy5HcnBjLlJvbGVzUmVzcG9uc2US",
            "UQoKSW5zZXJ0TXV0aRIiLlByb3RvLlJvbGVzLkdycGMuTXV0aVJvbGVzUmVx",
            "dWVzdBofLlByb3RvLlJvbGVzLkdycGMuUm9sZXNSZXNwb25zZWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.Roles.Grpc.Roles), global::Proto.Roles.Grpc.Roles.Parser, new[]{ "Id", "Name" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.Roles.Grpc.RolesRequest), global::Proto.Roles.Grpc.RolesRequest.Parser, new[]{ "Ids" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.Roles.Grpc.MutiRolesRequest), global::Proto.Roles.Grpc.MutiRolesRequest.Parser, new[]{ "Roles" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.Roles.Grpc.FailIdsResponse), global::Proto.Roles.Grpc.FailIdsResponse.Parser, new[]{ "Ids" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.Roles.Grpc.RolesResponse), global::Proto.Roles.Grpc.RolesResponse.Parser, new[]{ "Roles" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Roles : pb::IMessage<Roles> {
    private static readonly pb::MessageParser<Roles> _parser = new pb::MessageParser<Roles>(() => new Roles());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Roles> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.Roles.Grpc.RolesReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Roles() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Roles(Roles other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Roles Clone() {
      return new Roles(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Roles);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Roles other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Roles other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class RolesRequest : pb::IMessage<RolesRequest> {
    private static readonly pb::MessageParser<RolesRequest> _parser = new pb::MessageParser<RolesRequest>(() => new RolesRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RolesRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.Roles.Grpc.RolesReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesRequest(RolesRequest other) : this() {
      ids_ = other.ids_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesRequest Clone() {
      return new RolesRequest(this);
    }

    /// <summary>Field number for the "Ids" field.</summary>
    public const int IdsFieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_ids_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> ids_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Ids {
      get { return ids_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RolesRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RolesRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!ids_.Equals(other.ids_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= ids_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      ids_.WriteTo(output, _repeated_ids_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += ids_.CalculateSize(_repeated_ids_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RolesRequest other) {
      if (other == null) {
        return;
      }
      ids_.Add(other.ids_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ids_.AddEntriesFrom(input, _repeated_ids_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class MutiRolesRequest : pb::IMessage<MutiRolesRequest> {
    private static readonly pb::MessageParser<MutiRolesRequest> _parser = new pb::MessageParser<MutiRolesRequest>(() => new MutiRolesRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MutiRolesRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.Roles.Grpc.RolesReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MutiRolesRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MutiRolesRequest(MutiRolesRequest other) : this() {
      roles_ = other.roles_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MutiRolesRequest Clone() {
      return new MutiRolesRequest(this);
    }

    /// <summary>Field number for the "Roles" field.</summary>
    public const int RolesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Proto.Roles.Grpc.Roles> _repeated_roles_codec
        = pb::FieldCodec.ForMessage(10, global::Proto.Roles.Grpc.Roles.Parser);
    private readonly pbc::RepeatedField<global::Proto.Roles.Grpc.Roles> roles_ = new pbc::RepeatedField<global::Proto.Roles.Grpc.Roles>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto.Roles.Grpc.Roles> Roles {
      get { return roles_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MutiRolesRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MutiRolesRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!roles_.Equals(other.roles_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= roles_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      roles_.WriteTo(output, _repeated_roles_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += roles_.CalculateSize(_repeated_roles_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MutiRolesRequest other) {
      if (other == null) {
        return;
      }
      roles_.Add(other.roles_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            roles_.AddEntriesFrom(input, _repeated_roles_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class FailIdsResponse : pb::IMessage<FailIdsResponse> {
    private static readonly pb::MessageParser<FailIdsResponse> _parser = new pb::MessageParser<FailIdsResponse>(() => new FailIdsResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FailIdsResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.Roles.Grpc.RolesReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FailIdsResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FailIdsResponse(FailIdsResponse other) : this() {
      ids_ = other.ids_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FailIdsResponse Clone() {
      return new FailIdsResponse(this);
    }

    /// <summary>Field number for the "Ids" field.</summary>
    public const int IdsFieldNumber = 1;
    private static readonly pb::FieldCodec<string> _repeated_ids_codec
        = pb::FieldCodec.ForString(10);
    private readonly pbc::RepeatedField<string> ids_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Ids {
      get { return ids_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FailIdsResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FailIdsResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!ids_.Equals(other.ids_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= ids_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      ids_.WriteTo(output, _repeated_ids_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += ids_.CalculateSize(_repeated_ids_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FailIdsResponse other) {
      if (other == null) {
        return;
      }
      ids_.Add(other.ids_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ids_.AddEntriesFrom(input, _repeated_ids_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class RolesResponse : pb::IMessage<RolesResponse> {
    private static readonly pb::MessageParser<RolesResponse> _parser = new pb::MessageParser<RolesResponse>(() => new RolesResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RolesResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.Roles.Grpc.RolesReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesResponse(RolesResponse other) : this() {
      roles_ = other.roles_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RolesResponse Clone() {
      return new RolesResponse(this);
    }

    /// <summary>Field number for the "Roles" field.</summary>
    public const int RolesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Proto.Roles.Grpc.Roles> _repeated_roles_codec
        = pb::FieldCodec.ForMessage(10, global::Proto.Roles.Grpc.Roles.Parser);
    private readonly pbc::RepeatedField<global::Proto.Roles.Grpc.Roles> roles_ = new pbc::RepeatedField<global::Proto.Roles.Grpc.Roles>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto.Roles.Grpc.Roles> Roles {
      get { return roles_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RolesResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RolesResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!roles_.Equals(other.roles_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= roles_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      roles_.WriteTo(output, _repeated_roles_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += roles_.CalculateSize(_repeated_roles_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RolesResponse other) {
      if (other == null) {
        return;
      }
      roles_.Add(other.roles_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            roles_.AddEntriesFrom(input, _repeated_roles_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code