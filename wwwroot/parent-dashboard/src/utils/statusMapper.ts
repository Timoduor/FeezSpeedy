import { LoanStatus } from "../types";

// Map numeric status from backend to string
export const statusMap: Record<number, LoanStatus> = {
    0: "PENDING",
    1: "APPROVED",
    2: "REJECTED",
    3: "DISBURSED",
};

// Type guard to safely check keys
export function isStatusKey(key: any): key is keyof typeof statusMap {
    return Object.keys(statusMap).includes(String(key));
}