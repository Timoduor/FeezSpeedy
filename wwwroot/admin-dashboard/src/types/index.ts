export type LoanStatus = "PENDING" | "APPROVED" | "REJECTED" | "DISBURSED";

// Represents a fee request from a parent for a dependant
export interface FeeRequest {
    id: number;
    dependantName: string;
    parentName: string;
    schoolName: string;
    amount: number;            // Amount requested
    totalPayable?: number;     // Amount + interest if any
    interest?: number;         // Interest applicable
    status: LoanStatus;            // e.g., Pending, Approved, Rejected
    requestedOn?: string;
}

// Represents a disbursement made by admin
export interface Disbursement {
    id: number;
    dependantName: string;
    parentName: string;
    schoolName: string;
    amount: number;            // Amount disbursed
    paid: boolean;             // True if disbursed
    disbursedAt?: string;      // Timestamp
}

// Represents a registered parent
export interface Parent {
    id: number;
    fullName: string;
    email?: string;
}

// Represents a school
export interface School {
    id: number;
    name: string;
    location?: string;
}

// Optional: Dependants as separate entity if needed
export interface Dependant {
    id: number;
    fullName: string;
    parentId: number;
    schoolId: number;
    age?: number;
}