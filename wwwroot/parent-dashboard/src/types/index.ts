export type LoanStatus =
    | "PENDING"
    | "APPROVED"
    | "REJECTED"
    | "DISBURSED";

export interface FeeRequest {
    id: number;
    dependantName: string;
    parentName?: string;
    schoolName: string;
    amount: number;          // amount requested
    interest?: number;
    totalPayable: number;    // total after interest
    status: LoanStatus;      // change from number to string
    paymentMethod?: string;
    isPaid?: boolean;
    approvalMessage?: string;
    requestedOn?: string;
}

export interface Disbursement {
    id: number;
    dependantName: string;
    parentName: string;
    schoolName: string;
    amount: number;
    paid: boolean;
    disbursedAt?: string;
}

export interface Parent {
    id: number;
    fullName: string;
    email?: string;
}

export interface School {
    id: number;
    name: string;
    location?: string;
}

export interface Dependant {
    id: number;
    fullName: string;
    admissionNumber: string;
    classLevel: string;
    schoolId: number;
    parentId: string; // must match backend (Identity uses string)
}
