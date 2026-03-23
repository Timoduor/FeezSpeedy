import React, { useEffect, useState } from "react";
import { getAdminDashboardData } from "../services/adminApi";
import SummaryCards from "../components/SummaryCards";
import FeeRequestsTable from "../components/FeeRequestsTable";
import DisbursementsTable from "../components/DisbursementsTable";
import ParentsTable from "../components/ParentsTable";
import SchoolsTable from "../components/SchoolsTable";
import { FeeRequest, Disbursement, Parent, School } from "../types";

interface Props {
    feeRequests: FeeRequest[];
    disbursements: Disbursement[];
    parents: Parent[];
    schools: School[];
}

const AdminDashboard: React.FC = () => {
    const [feeRequests, setFeeRequests] = useState<FeeRequest[]>([]);
    const [disbursements, setDisbursements] = useState<Disbursement[]>([]);
    const [parents, setParents] = useState<Parent[]>([]);
    const [schools, setSchools] = useState<School[]>([]);

    useEffect(() => {
        getAdminDashboardData().then(data => {
            setFeeRequests(data.feeRequests);
            setDisbursements(data.disbursements);
            setParents(data.parents);
            setSchools(data.schools);
        });
    }, []);
    // ---- Calculations ----
    const approved = feeRequests.filter(f => f.status === "APPROVED").length;
    const pending = feeRequests.filter(f => f.status === "PENDING").length;
    const rejected = feeRequests.filter(f => f.status === "REJECTED").length;

    const outstanding = feeRequests
        .filter(f => f.status === "APPROVED")
        .reduce((s, f) => s + (f.totalPayable || 0), 0);

    const totalDisbursed = disbursements.reduce((s, d) => s + d.amount, 0);

    return (
        <div className="container-fluid px-4 mt-4">
            <h2 className="mb-3">Admin Dashboard</h2>

            {/* SUMMARY CARDS */}
            <SummaryCards
                cards={[
                    { title: "Total Fee Requests", value: feeRequests.length, variant: "primary" },
                    { title: "Approved Requests", value: approved, variant: "success" },
                    { title: "Pending Requests", value: pending, variant: "warning" },
                    { title: "Rejected Requests", value: rejected, variant: "danger" },
                    { title: "Total Payable", value: outstanding, variant: "secondary" },
                    { title: "Total Disbursed", value: totalDisbursed, variant: "primary" },
                    { title: "Total Parents", value: parents.length, variant: "success" },
                    { title: "Total Schools", value: schools.length, variant: "warning" },
                ]}
            />

            {/* FEE REQUESTS */}
            <div className="mt-4">
                <h5 className="mb-2">Fee Requests</h5>
                <FeeRequestsTable feeRequests={feeRequests} showAdminColumns />
            </div>

            {/* DISBURSEMENTS */}
            <div className="mt-4">
                <h5 className="mb-2">Disbursements</h5>
                <DisbursementsTable disbursements={disbursements} />
            </div>

            {/* SYSTEM DATA */}
            <div className="row mt-4">
                <div className="col-md-6">
                    <h5 className="mb-2">Parents</h5>
                    <ParentsTable parents={parents} />
                </div>
                <div className="col-md-6">
                    <h5 className="mb-2">Schools</h5>
                    <SchoolsTable schools={schools} />
                </div>
            </div>
        </div>
    );
};



export default AdminDashboard;