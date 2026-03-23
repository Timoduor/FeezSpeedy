import axios from "axios";
import { FeeRequest, Disbursement, Parent, School } from "../types";

const API = axios.create({
    baseURL: "/api", // relative path, works in prod
    withCredentials: true
});

export const getAdminDashboardData = async () => {
    const [
        feeRequests,
        disbursements,
        parents,
        schools
    ] = await Promise.all([
        API.get<FeeRequest[]>("/fee-requests"),
        API.get<Disbursement[]>("/disbursements"),
        API.get<Parent[]>("/parents"),
        API.get<School[]>("/schools"),
    ]);

    return {
        feeRequests: feeRequests.data,
        disbursements: disbursements.data,
        parents: parents.data,
        schools: schools.data
    };
};

export const approveRequest = (id: number) =>
    API.post(`/fee-requests/${id}/approve`);

export const rejectRequest = (id: number, reason?: string) =>
    API.post(`/fee-requests/${id}/reject`, { reason });

export const disburseRequest = (id: number) =>
    API.post(`/fee-requests/${id}/disburse`);