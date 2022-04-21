import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import { formErrorMessage } from '../modules/errorMessageFormer';

export class UserRow extends Component {
    static displayName = UserRow.name;

    constructor(props) {
        super(props);

        this.state = {
            username: sessionStorage.getItem('username'),
            id: sessionStorage.getItem('id'),
            token: sessionStorage.getItem('token')
        };
    }

    async deleteUser(id, username) {
        await fetch(`../api/users/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return formErrorMessage(response);
                }

                if (username === this.state.username) {
                    window.location.href = '/logout';
                }
            })
            .catch(error => alert(error.message));
    }

    render() {
        const id = this.props.id;
        const username = this.props.username;
        const role = this.props.role;
        const profilePicture = this.props.profilePicture;
        
        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${profilePicture}` } alt="user-profile-pic" width="75" />
            </td>
            <td>{ id }</td>
            <td>{ username }</td>
            <td>{ role }</td>
            <td>
            {(this.props.calledFromAdmin || (this.state.id === id)) &&
                <div className="text-center d-flex justify-content-around">
                    <NavLink className="btn btn-sm btn-info text-white" to={{
                        pathname: `/users/edit/${id}`,
                        state: {
                            username: username,
                            role: role,
                            profilePicture: profilePicture
                        }
                    }}>Edit</NavLink>
                    <button className="btn btn-sm btn-danger" onClick={
                        async () => { 
                            await this.deleteUser(id, username); 
                            await this.props.getUsers(); 
                        }
                    }>Delete</button>
                </div>
            }
            </td>
        </tr>;
    }
}