﻿/** 
 * Copyright (C) 2015 smndtrl
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Strilanc.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libtextsecure.messages
{
    /**
     * Group information to include in TextSecureMessages destined to groups.
     *
     * This class represents a "context" that is included with textsecure messages
     * to make them group messages.  There are three types of context:
     *
     * 1) Update -- Sent when either creating a group, or updating the properties
     *    of a group (such as the avatar icon, membership list, or title).
     * 2) Deliver -- Sent when a message is to be delivered to an existing group.
     * 3) Quit -- Sent when the sender wishes to leave an existing group.
     *
     * @author
     */
    public class TextSecureGroup
    {

        public enum Type
        {
            UNKNOWN,
            UPDATE,
            DELIVER,
            QUIT
        }

        private readonly byte[] groupId;
        private readonly Type type;
        private readonly May<String> name;
        private readonly May<IList<String>> members;
        private readonly May<TextSecureAttachment> avatar;


        /**
         * Construct a DELIVER group context.
         * @param groupId
         */
        public TextSecureGroup(byte[] groupId)
                 : this(Type.DELIVER, groupId, null, null, null)
        {
        }

        /**
         * Construct a group context.
         * @param type The group message type (update, deliver, quit).
         * @param groupId The group ID.
         * @param name The group title.
         * @param members The group membership list.
         * @param avatar The group avatar icon.
         */
        public TextSecureGroup(Type type, byte[] groupId, String name,
                               IList<String> members,
                               TextSecureAttachment avatar)
        {
            this.type = type;
            this.groupId = groupId;
            this.name = new May<String>(name);
            this.members = new May<IList<String>>(members);
            this.avatar = new May<TextSecureAttachment>(avatar);
        }

        public byte[] getGroupId()
        {
            return groupId;
        }

        public Type getType()
        {
            return type;
        }

        public May<String> getName()
        {
            return name;
        }

        public May<IList<String>> getMembers()
        {
            return members;
        }

        public May<TextSecureAttachment> getAvatar()
        {
            return avatar;
        }

        public  Builder newUpdateBuilder()
        {
            return new Builder(Type.UPDATE);
        }

        public  Builder newBuilder(Type type)
        {
            return new Builder(type);
        }

        public class Builder
        {

            private Type type;
            private byte[] id;
            private String name;
            private List<String> members;
            private TextSecureAttachment avatar;

            internal Builder(Type type)
            {
                this.type = type;
            }

            public Builder withId(byte[] id)
            {
                this.id = id;
                return this;
            }

            public Builder withName(String name)
            {
                this.name = name;
                return this;
            }

            public Builder withMembers(List<String> members)
            {
                this.members = members;
                return this;
            }

            public Builder withAvatar(TextSecureAttachment avatar)
            {
                this.avatar = avatar;
                return this;
            }

            public TextSecureGroup build()
            {
                if (id == null) throw new Exception("No group ID specified!");

                if (type == Type.UPDATE && name == null && members == null && avatar == null)
                {
                    throw new Exception("Group update with no updates!");
                }

                return new TextSecureGroup(type, id, name, members, avatar);
            }

        }
    }
        
}
