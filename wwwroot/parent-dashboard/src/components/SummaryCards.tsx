import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";

// Card item interface
export interface SummaryCardProps {
    title: string;
    value: number | string;
    variant?: "primary" | "success" | "danger" | "warning";
    id?: string;
}

// Props for SummaryCards component
export interface SummaryCardsProps {
    cards: SummaryCardProps[];
}

const SummaryCards: React.FC<SummaryCardsProps> = ({ cards }) => {
    return (
        <div className="row g-3 mt-3">
            {cards.map((card, idx) => (
                <div className="col-md-4" key={idx}>
                    <div className="card shadow-sm border-0">
                        <div className="card-body">
                            <small className="text-muted">{card.title}</small>
                            <h4 className={`fw-bold mt-1 text-${card.variant ?? "primary"}`}>
                                {typeof card.value === "number" ? `KES ${card.value.toLocaleString("en-KE")}` : card.value}
                            </h4>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default SummaryCards;