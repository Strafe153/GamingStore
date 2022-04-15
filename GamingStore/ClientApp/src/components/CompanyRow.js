import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

export class CompanyRow extends Component {
    static displayName = CompanyRow.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token')
        };
    }

    async deleteCompany(id) {
        await fetch(`../api/companies/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    render() {
        const companyId = this.props.id;
        const name = this.props.name;
        const icon = this.props.icon;

        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${icon}` } alt="company-icon" width="100" />
            </td>
            <td>{ companyId }</td>
            <td>{ name }</td>
            <td className="text-center d-flex justify-content-around">
                <NavLink className="btn btn-sm btn-info text-white" to={{
                    pathname: `/companies/edit/${companyId}`,
                    state: {
                        name: name,
                        icon: icon
                    }
                }}>Edit</NavLink>
                <button onClick={
                    async () => { 
                        await this.deleteCompany(companyId); 
                        await this.props.getCompanies(); 
                    }
                } className="btn btn-sm btn-danger">Delete</button>
            </td>
        </tr>;
    }
}