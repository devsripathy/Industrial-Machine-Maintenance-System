export interface UserDto {
  id: number;
  username: string;
  email: string;
  fullName: string;
  roleName: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: UserDto;
}

export interface MachineDependencyDto {
  machineId: number;
  dependsOnMachineId: number;
  dependsOnMachineName: string;
  dependsOnMachineCode: string;
  dependencyType: string;
}

export interface MachineDto {
  id: number;
  name: string;
  code: string;
  serialNumber: string;
  machineTypeId: number;
  machineTypeCode: string;
  machineTypeName: string;
  criticality: string;
  criticalityValue: number;
  status: string;
  statusValue: number;
  model: string;
  location: string;
  specificationsJson: string;
  qrCodeData: string;
  installDate: string | null;
  createdAt: string;
  updatedAt: string | null;
  dependencies: MachineDependencyDto[];
}
