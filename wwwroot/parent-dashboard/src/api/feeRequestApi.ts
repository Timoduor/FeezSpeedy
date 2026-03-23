// parent-dashboard/src/api/feeRequestApi.ts
import { FeeRequest } from "../types";


export const createFeeRequest = async (data: any) => {

    const res = await fetch("/api/feerequests", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    return res.json();
};

export const fetchFeeRequests = async (): Promise<FeeRequest[]> => {
    const res = await fetch("/api/feerequests");
    if (!res.ok) throw new Error("Failed to fetch fee requests");
    return res.json();
};

export const addFeeRequest = async (req: FeeRequest): Promise<FeeRequest> => {
    const res = await fetch("/api/feerequests", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(req),
    });
    if (!res.ok) throw new Error("Failed to add fee request");
    return res.json();
};